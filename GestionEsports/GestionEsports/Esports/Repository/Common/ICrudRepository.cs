using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Repository;

public interface ICrudRepository<TKey, Tvalue> {
    IEnumerable<Tvalue?> GetAll(int page, int pageSize, bool isDeleted, string? busqueda);
    
    Tvalue? GetById(TKey id);

    Result<Tvalue, DomainError> Create(Tvalue jugador);
    
    Result<Tvalue, DomainError> Update(TKey id, Tvalue jugador);

    Tvalue? Delete(TKey id, bool isLogic);
    
    bool DeleteAll();

    Result<Tvalue, DomainError> Restore(TKey id);
}