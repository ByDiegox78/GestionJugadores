using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;

namespace GestionEsports.Esports.Service.Import.Export;

public interface IImportExportService {
    Result<int, DomainError> ExportarDatos(IEnumerable<Jugador> jugadors, string path);
    Result<IEnumerable<Jugador>, DomainError> ImportarDatos(string path);
    Result<int, DomainError> ExportarDatosSistema(IEnumerable<Jugador> jugadores);
    Result<IEnumerable<Jugador>, DomainError> ImportarDatosSistema(string path);
}