using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;
using GestionEsports.Esports.Storage.Common;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Esports.Service.Import.Export;

public class ImportExportService(IStorage<Jugador> storage) : IImportExportService{
    private readonly ILogger _logger = Log.ForContext<ImportExportService>();
    public Result<int, DomainError> ExportarDatos(IEnumerable<Jugador> jugadors, string path) {
        _logger.Information("Exportando los datos del sistema a un archivo {Path}", path);
        var list = jugadors.ToList();
        return storage.Salvar(list, path)
            .Map(r => list.Count);
    }
    public Result<IEnumerable<Jugador>, DomainError> ImportarDatos(string path) {
        _logger.Information("Importando los datos del archivo {Path}", path);
        return storage.Cargar(path);
    }
    public Result<int, DomainError> ExportarDatosSistema(IEnumerable<Jugador> jugadores) {
        _logger.Information("Exportando los datos del sistema a un archivo");
        return ExportarDatos(jugadores, string.Empty);
    }
    public Result<IEnumerable<Jugador>, DomainError> ImportarDatosSistema(string path) {
        _logger.Information("Importando los datos del archivo {Path}", path);
        return ImportarDatos(path);
    }
}