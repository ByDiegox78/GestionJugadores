using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using GestionEsports.Esports.Config;
using GestionEsports.Esports.Dto;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Mapper;
using GestionEsports.Esports.Models;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Esports.Storage.Json;

public class JugadorJsonStorage : IStorageJugadorJson {
    private readonly ILogger _logger = Log.ForContext<JugadorJsonStorage>();

    private readonly JsonSerializerOptions _options = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public JugadorJsonStorage() {
        InitStorage();
    }
    public Result<IEnumerable<Jugador>, DomainError> Cargar(string path) {
        _logger.Debug("Cargando los datos desde el archivo {Path}", path);
        if (!Path.Exists(path))
            return Result.Failure<IEnumerable<Jugador>, DomainError>(StorageErrors.FileNotFount(path));
        try {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var dtos = JsonSerializer.Deserialize<List<JugadorDto>>(json, _options);
            if (dtos == null) 
                return Result.Failure<IEnumerable<Jugador>, DomainError>(
                    StorageErrors.InvalidFormat("No se pudo deserializar los DTOs"));
            return Result.Success<IEnumerable<Jugador>, DomainError>(dtos.Select(d => d.ToModel()));

        } catch (Exception e) {
            _logger.Error(e,"Error al cargar los items del archivo {path}", path);
            return Result.Failure<IEnumerable<Jugador>, DomainError>(StorageErrors.ReadError(e.Message));
        }
    }

    public Result<bool, DomainError> Salvar(IEnumerable<Jugador> items, string path) {
        try {
            _logger.Debug("Guardando los datos del archivo {Path}", path);
            var dtos = items.Select(p => p.ToDto());
            var json = JsonSerializer.Serialize(dtos, _options);
            File.WriteAllText(path, json, Encoding.UTF8);
            return Result.Success<bool, DomainError>(true);
        } catch (Exception e) {
            _logger.Error(e,"Error al guardar los items en el archivo {Path}", path);
            return Result.Failure<bool, DomainError>(StorageErrors.WriteErrir(e.Message));
        }
    }
    private void InitStorage() {
        if (Directory.Exists(AppConfig.DataFolder)) 
            return;
        Directory.CreateDirectory(AppConfig.DataFolder);
    }
}