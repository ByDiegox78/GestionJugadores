using System.IO.Compression;
using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;
using GestionEsports.Esports.Storage.Common;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Esports.Service.Backup;

public class BackupService(IStorage<Jugador> storage, string? defaultBackupDirectory = null) : IBackupService {
    private readonly ILogger _logger = Log.ForContext<BackupService>();
    
    public Result<string, DomainError> RealizarBackup(IEnumerable<Jugador> jugadores, string? customDirectory = null) {
        var dir = customDirectory ?? defaultBackupDirectory ??
             throw new InvalidOperationException("No se dijo un directorio");
        _logger.Information("Realizando backup en la ruta {Path}", dir);
        var list = jugadores.ToList();
        if (!list.Any()) {
            _logger.Warning("No se han encontrado jugadores para realizar el backup");
            return Result.Failure<string, DomainError>(BackupErrors.CreationError("No se han encontrado jugadores para realizar el backup"));
        }
        try {
            Directory.CreateDirectory(dir);
            
        } catch (Exception e) {
            _logger.Error(e,"Error al crear el directorio del backup {Path}", dir);
            return Result.Failure<string, DomainError>(BackupErrors.DirectoryError($"No se pudo crear el directorio del backup: {dir}"));
        }
        var tempDir = Path.Combine(Path.GetTempPath(), $"backup-{Guid.NewGuid()}");
        var dataDir = Path.Combine(tempDir, "data");
        Directory.CreateDirectory(dataDir);
        try {
            var json = Path.Combine(dataDir, "jugadores.json");
            var salvarBackup = storage.Salvar(list, json);
            if (salvarBackup.IsFailure) {
                BackupErrors.CreationError("No se pudo crear el backup, Error de serializacion");
                return Result.Failure<string, DomainError>(
                    BackupErrors.CreationError("No se pudo crear el backup, Error de serializacion"));
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var zipPath = Path.Combine(dir, $"{date}-back.zip");
            try {
                ZipFile.CreateFromDirectory(tempDir, zipPath);
            }
            catch (Exception e) {
                _logger.Error(e, "Error al crear el zip");
                return Result.Failure<string, DomainError>(BackupErrors.CreationError("No se pudo crear el zip"));
            }

            _logger.Information("Backup realizado correctamente en {Path}", zipPath);
            return Result.Success<string, DomainError>(zipPath);
        }
        finally {
            if (Directory.Exists(tempDir)) {
                Directory.Delete(tempDir, true);
                _logger.Information("Directorio temporal eliminado");
            }
        }
    }

    public Result<IEnumerable<Jugador>, DomainError> RestaurarBackup(string archivo, string? customDirectory = null) {
        _logger.Information("Restaurando backup desde {Path}", archivo);
        if (!File.Exists(archivo)) {
            _logger.Warning("El archivo {Path} no existe", archivo);
            return Result.Failure<IEnumerable<Jugador>, DomainError>(BackupErrors.FileNotFound(archivo));
        }
        var tempDir = Path.Combine(Path.GetTempPath(), $"backup-{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);
        try {
            try {
                ZipFile.ExtractToDirectory(archivo, tempDir);
            }
            catch (Exception e) {
                _logger.Error(e, "Error al descomprimir el zip");
                return Result.Failure<IEnumerable<Jugador>, DomainError>(
                    BackupErrors.InvalidBackupFile("No se pudo descomprimir el zip"));
            }

            var dataDir = Path.Combine(tempDir, "data");
            var jsonPath = Path.Combine(dataDir, "jugadores.json");
            if (!File.Exists(jsonPath)) {
                _logger.Warning("El archivo {Path} no existe", jsonPath);
                return Result.Failure<IEnumerable<Jugador>, DomainError>(
                    BackupErrors.InvalidBackupFile("El archivo de backup no contiene el archivo jugadores"));
            }

            var cargarResult = storage.Cargar(jsonPath);
            if (cargarResult.IsFailure) {
                _logger.Error("Error al serializar los jugadores");
                return Result.Failure<IEnumerable<Jugador>, DomainError>(
                    BackupErrors.InvalidBackupFile("No se pudo cargar los jugadores"));
            }
            var jugadoresList = cargarResult.Value.ToList();
            _logger.Information("Backup restaurado correctamente");
            return Result.Success<IEnumerable<Jugador>, DomainError>(jugadoresList);
        } finally {
            if (Directory.Exists(tempDir)) {
                Directory.Delete(tempDir, true);
                _logger.Information("Directorio temporal eliminado");
            }
        }
    }
    public IEnumerable<string> ListarBackups(string? customDirectory = null) {
        var dir = customDirectory ?? defaultBackupDirectory;
        if (dir == null || !Directory.Exists(dir)) 
            return Enumerable.Empty<string>();
        return Directory.GetFiles(dir, "*.zip")
            .OrderByDescending(File.GetCreationTime);
    }
    public Result<string, DomainError> RealizarBackupSistema(IEnumerable<Jugador> jugadores) {
        return RealizarBackup(jugadores);
    }
    public Result<int, DomainError> RestaurarBackupSistema(string archivo, Func<bool> deleteAllCallback, Func<Jugador, Result<Jugador, DomainError>> createCallback) {
        _logger.Information("Iniciando restauracion completa desde {archivo}", archivo);
        var deleteResult = deleteAllCallback();
        if (!deleteResult) {
            _logger.Warning("No se pudieron borrar los datos existentes");
            return Result.Failure<int, DomainError>(BackupErrors.RestorationError("No se pudieron borrar los datos existentes"));
        }

        return RestaurarBackup(archivo)
            .Bind(jugador => {
                var count = 0;
                DomainError? primerError = null;
                foreach (var j in jugador) {
                    var result = createCallback(j);
                    if (result.IsSuccess)
                        count++;
                    else if (primerError == null)
                        primerError = result.Error;
                }
                if (primerError != null || count == 0)
                    return Result.Failure<int, DomainError>(primerError);
                _logger.Information("Restauración completada. Total registros: {count}", count);
                return Result.Success<int, DomainError>(count);
            });
    }
}