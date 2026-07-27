using GestionEsports.Esports.Models;

namespace GestionEsports.Factory;

public static class JugadorFactory {
    public static IEnumerable<Jugador> Seed() {
        var now = DateTime.UtcNow;
        var today = now.Date;
        return new List<Jugador>() {
            new Jugador { }
        };
    }
}