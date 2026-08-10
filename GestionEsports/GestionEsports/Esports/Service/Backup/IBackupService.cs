using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;

namespace GestionEsports.Esports.Service.Backup;

public interface IBackupService {
    Result<string, DomainError> RealizarBackup(IEnumerable<Jugador> jugadores, string? customDirectory = null);
    Result<IEnumerable<Jugador>, DomainError> RestaurarBackup(string archivo, string? customDirectory = null);
    IEnumerable<string> ListarBackups(string? customDirectory = null);
    Result<string, DomainError> RealizarBackupSistema(IEnumerable<Jugador> jugadores);
    Result<int, DomainError> RestaurarBackupSistema(
        string archivo,Func<bool> deleteAllCallback, Func<Jugador, Result<Jugador, DomainError>> createCallback);
}