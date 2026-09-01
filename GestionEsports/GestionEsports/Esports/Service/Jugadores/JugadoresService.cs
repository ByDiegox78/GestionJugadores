using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;
using GestionEsports.Esports.Validator;
using GestionEsports.Repository;
using Serilog;
using Vehiculos.Cache;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Esports.Service.Jugadores;

public class JugadoresService(IJugadorRepository repository, IValidator<Jugador> validator, ICached<int, Jugador> cache) : IJugadoresService {
    private readonly ILogger _logger = Log.ForContext<JugadoresService>();

    
    public IEnumerable<Jugador> GetAll(int page, int pageSize, bool isDeleted, string? busqueda) {
        var res = repository.GetAll(page, pageSize, isDeleted, busqueda);
        return res;
    }

    public Jugador? GetById(int id) {
        _logger.Debug("Obteniendo jugador con id {Id}", id);
        if (cache.Get(id) is { } jugador) 
            return jugador;
        if (repository.GetById(id) is not { } jugadorRepo) return null;
        cache.Add(id, jugadorRepo);
        return jugadorRepo;
    }

    public Result<Jugador, DomainError> Create(Jugador jugador) {
        _logger.Debug("Creando jugador");
        return ValidarJugador(jugador)
            .Bind(b => repository.Create(b))
            .Tap(t => cache.Add(t.Id, t));
    }

    public Result<Jugador, DomainError> Update(int id, Jugador jugador) {
        _logger.Debug("Actualizando jugador con id {Id}", id);
        return ComprobarExistencia(id)
            .Tap(t => { cache.Remove(id); })
            .Bind(b => ValidarJugador(jugador))
            .Bind(b => repository.Update(id, b));
    }

    public Result<Jugador, DomainError> Delete(int id, bool isLogic) {
        _logger.Warning("Eliminando jugador con id {Id}", id);
        return ComprobarExistencia(id)
            .Tap(t => { cache.Remove(id); })
            .Map(b => repository.Delete(id, isLogic)!);
    }

    public bool DeleteAll() {
        _logger.Warning("Eliminando todos los jugadores");
        return repository.DeleteAll();
    }

    public Result<Jugador, DomainError> Restore(int id) {
        _logger.Information("Restaurando jugador con id {Id}", id);
        return repository.Restore(id);
    }

    public Result<Jugador, DomainError> GetByPlayerName(string name) {
        _logger.Debug("Obteniendo jugador con nombre {Name}", name);
        var jugador = repository.GetByPlayerName(name);
        if (jugador is null) {
            return Result.Failure<Jugador, DomainError>(
                JugadorErrors.NotFoundByName(name));
        }
        return Result.Success<Jugador, DomainError>(jugador);
    }

    public Result<IEnumerable<Jugador>, DomainError> GetByRol(string rol) {
        _logger.Debug("Obteniendo jugadores con rol {Rol}", rol);
        if (repository.GetByRol(rol) is { } jugador)
            return Result.Success<IEnumerable<Jugador>, DomainError>(jugador);
        return Result.Failure<IEnumerable<Jugador>, DomainError>(JugadorErrors.NotFoundByRol(rol));
    }

    public Result<IEnumerable<Jugador>, DomainError> GetByTeam(string team) {
        
        _logger.Debug("Obteniendo jugadores con equipo {Team}", team);
        if (repository.GetByRol(team) is { } jugador)
            return Result.Success<IEnumerable<Jugador>, DomainError>(jugador);
        return Result.Failure<IEnumerable<Jugador>, DomainError>(JugadorErrors.NotFoundByTeam(team));    }

    public Result<bool, DomainError> ExisteJugador(string jugador) {
        _logger.Debug("Verificando si existe el jugador con nombre {Nombre}", jugador);
        return repository.ExisteJugador(jugador);
    }

    public (Jugador? jugador1, Jugador? jugador2) CompararJugadores(int id1, int id2) {
        _logger.Debug("Comparando jugadores con ids {Id1} y {Id2}", id1, id2);
        return repository.CompararJugadores(id1, id2);
    }

    private Result<Jugador, DomainError> ValidarJugador(Jugador jugador) {
        _logger.Debug("Validando jugador");
        var val = validator.Validar(jugador);
        return val.IsFailure
            ? Result.Failure<Jugador, DomainError>(val.Error)
            : Result.Success<Jugador, DomainError>(jugador);
    }

    private Result<Jugador, DomainError> ComprobarExistencia(int id) {
        return repository.GetById(id) is { } v
            ? Result.Success<Jugador, DomainError>(v)
            : Result.Failure<Jugador, DomainError>(JugadorErrors.NotFound(id));
    }
}