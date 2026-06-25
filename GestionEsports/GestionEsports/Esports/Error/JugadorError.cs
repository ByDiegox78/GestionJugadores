using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Esports.Error;

public abstract record JugadorError(string Message) : DomainError(Message) {
    public sealed record Validation(IEnumerable<string> Errors) 
        : JugadorError($"Se han detectado errores de validación en la entidad:{Environment.NewLine}• {string.Join($"{Environment.NewLine}• ", Errors)}");
}

public static class JugadorErrors {
    public static DomainError Validation(IEnumerable<string> errors) {
        return new JugadorError.Validation(errors);
    }
}