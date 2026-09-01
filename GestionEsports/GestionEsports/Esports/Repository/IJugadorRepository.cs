using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;

namespace GestionEsports.Repository;

public interface IJugadorRepository: ICrudRepository<int, Jugador> {
    Jugador? GetByPlayerName(string name);
    IEnumerable<Jugador>? GetByRol(string rol);
    IEnumerable<Jugador>? GetByTeam(string team);
    Result<bool, DomainError> ExisteJugador(string jugador);
    (Jugador? jugador1, Jugador? jugador2) CompararJugadores(int id1, int id2);

}