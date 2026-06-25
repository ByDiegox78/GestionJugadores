using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Esports.Storage.Common;

public interface IStorage<T> {
    Result<IEnumerable<T>, DomainError> Cargar(string path);
    Result<bool, DomainError> Salvar(IEnumerable<T> items, string path);
}