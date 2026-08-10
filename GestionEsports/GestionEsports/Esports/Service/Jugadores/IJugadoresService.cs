using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;

namespace GestionEsports.Esports.Service.Jugadores;

public interface IJugadoresService {
    IEnumerable<Jugador> GetAll(int page, int pageSize, bool isDeleted, string? busqueda);
    Jugador? GetById(int id);
    Result<Jugador, DomainError> Create(Jugador jugador);
    Result<Jugador, DomainError> Update(int id, Jugador jugador);
    Result<Jugador, DomainError> Delete(int id, bool isLogic);
    bool DeleteAll();
    Result<Jugador, DomainError> Restore(int id);
    Jugador? GetByPlayerName(string name);
    IEnumerable<Jugador>? GetByRol(string rol);
    IEnumerable<Jugador>? GetByTeam(string team);
    bool ExisteJugador(string jugador);
    (Jugador? jugador1, Jugador? jugador2) CompararJugadores(int id1, int id2);
}