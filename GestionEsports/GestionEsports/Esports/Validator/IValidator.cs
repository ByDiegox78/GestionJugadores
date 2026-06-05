using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Esports.Validator;

public interface IValidator<T> {
    Result<T, DomainError> Validar(T entity);
}