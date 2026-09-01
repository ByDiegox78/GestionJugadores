using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Esports.Error;

public abstract record JugadorError(string Message) : DomainError(Message) {
    public sealed record Validation(IEnumerable<string> Errors) 
        : JugadorError($"Se han detectado errores de validación en la entidad:{Environment.NewLine}• {string.Join($"{Environment.NewLine}• ", Errors)}");

    public sealed record NameAlreadyExists(string Nombre)
        : JugadorError($"Conflicto de integridad: El jugador con el nombre: {Nombre} ya esta registrado en el sistema");
    public sealed record DataBaseError(string Details)
        : JugadorError($"Error de Base de datos: {Details}");
    public sealed record NotFound(int Id)
        : JugadorError($"No se encontró: El jugador con el ID {Id} no existe en el sistema");
    public sealed record NotFoundByName(string Name)
        : JugadorError($"No se encontró: El jugador con el NOMBRE {Name} no existe en el sistema");
    public sealed record NotFoundByRol(string Rol)
        : JugadorError($"No se encontraron jugadores para el rol: {Rol}");

    public sealed record NotFoundByTeam(string Team)
        : JugadorError($"No se encontraron jugadores para el equipo: {Team}");
}

public static class JugadorErrors {
    public static DomainError Validation(IEnumerable<string> errors) {
        return new JugadorError.Validation(errors);
    }
    public static DomainError NameAlreadyEists(string name) {
        return new JugadorError.NameAlreadyExists(name);
    }
    public static DomainError DataBaseError(string details) {
        return new JugadorError.DataBaseError(details);
    }
    public static DomainError NotFound(int id) {
        return new JugadorError.NotFound(id);
    }
    public static DomainError NotFoundByName(string name) {
        return new JugadorError.NotFoundByName(name);
    }

    public static DomainError NotFoundByRol(string rol) {
        return new JugadorError.NotFoundByRol(rol);
    }

    public static DomainError NotFoundByTeam(string team) {
        return new JugadorError.NotFoundByTeam(team);
    }
}