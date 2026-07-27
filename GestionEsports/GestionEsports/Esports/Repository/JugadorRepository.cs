using CSharpFunctionalExtensions;
using GestionEsports.Entity;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Mapper;
using GestionEsports.Esports.Models;
using GestionEsports.Factory;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Repository;

public class JugadorRepository: IJugadorRepository {
    private readonly AppDbContext _context;
    private readonly ILogger _logger = Log.ForContext<JugadorRepository>();

    public JugadorRepository(AppDbContext context, bool dropData = false, bool seedData = false) {
        _context = context;
        if (dropData) _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        if (seedData && !_context.Jugador.Any()) {
            _logger.Information("Sembrando jugadore...");
            foreach (var j in JugadorFactory.Seed()) {
                Create(j);
            }
        }
    }
    
    public IEnumerable<Jugador> GetAll(int page, int pageSize, bool isDeleted, string? busqueda) {
        var consulta = _context.Jugador.AsQueryable();
        if (!isDeleted)
            consulta = consulta.Where(v => v.IsDeleted == false);
        if (!string.IsNullOrWhiteSpace(busqueda)) 
            consulta = consulta.Where(j => j.PlayerName.ToLower().Contains(busqueda.ToLower()));
        return consulta.OrderBy(j => j.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable()
            .Select(j => j.ToModel());
    }

    public Jugador? GetById(int id) {
        try {
            return _context.Jugador.FirstOrDefault(p => p.Id == id)?.ToModel();
        }
        catch (Exception e) {
            _logger.Error("No se a encontrado ningun jugador con id: {Id}", id);
            return null;
        }
    }

    public Result<Jugador, DomainError> Create(Jugador jugador) {
        if (ExisteJugador(jugador.PlayerName)) 
            return Result.Failure<Jugador, DomainError>(JugadorErrors.NameAlreadyEists(jugador.PlayerName));
        jugador = jugador with {
            Id = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        try {
            var entity = jugador.ToEntity();
            _context.Jugador.Add(entity);
            _context.SaveChanges();
            return Result.Success<Jugador, DomainError>(GetById(entity.Id)!);
        } catch (Exception e) {
            _logger.Error(e, "Error al crear el jugador");
            return Result.Failure<Jugador, DomainError>(JugadorErrors.DataBaseError(e.Message));
        }
    }

    public Result<Jugador, DomainError> Update(int id, Jugador jugador) {
        var entity = _context.Jugador.FirstOrDefault(j => j.Id == id);
        if (entity == null) 
            return Result.Failure<Jugador, DomainError>(JugadorErrors.NotFound(id));
        entity.PlayerName = jugador.PlayerName;
        entity.Team = jugador.Team;
        entity.Position = jugador.Position;
        entity.Games = jugador.Games;
        entity.WinRate = jugador.WinRate;
        entity.KDA = jugador.KDA;
        entity.AvgKills = jugador.AvgKills;
        entity.AvgDeaths = jugador.AvgDeaths;
        entity.AvgAssists = jugador.AvgAssists;
        entity.CSPerMin = jugador.CSPerMin;
        entity.GoldPerMin = jugador.GoldPerMin;
        entity.KPPercent = jugador.KPPercent;
        entity.DamagePercent = jugador.DamagePercent;
        entity.DPM = jugador.DPM;
        entity.VSPM = jugador.VSPM;
        entity.AvgWPM = jugador.AvgWPM;
        entity.AvgWCPM = jugador.AvgWCPM;
        entity.AvgVWPM = jugador.AvgVWPM;
        entity.GD15 = jugador.GD15;
        entity.CSD15 = jugador.CSD15;
        entity.XPD15 = jugador.XPD15;
        entity.FBPercent = jugador.FBPercent;
        entity.FBVictim = jugador.FBVictim;
        entity.PentaKills = jugador.PentaKills;
        entity.SoloKills = jugador.SoloKills;
        entity.Country = jugador.Country;
        entity.FlashKeybind = jugador.FlashKeybind;
        entity.UpdatedAt = DateTime.UtcNow;
        try {
            _context.SaveChanges();
            return Result.Success<Jugador, DomainError>(GetById(id)!);
        } catch (Exception e) {
            _logger.Error(e,"Error al actualizar el jugador");
            return Result.Failure<Jugador, DomainError>(JugadorErrors.DataBaseError(e.Message));
        }
    }

    public Jugador? Delete(int id, bool isLogic) {
        try {
            var entity = _context.Jugador.FirstOrDefault(j => j.Id == id);
            if (entity == null)
                return null;
            if (isLogic) {
                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                return GetById(id);
            }
            _context.Jugador.Remove(entity);
            _context.SaveChanges();
            return entity.ToModel();
        } catch (Exception e) {
            _logger.Error(e,"Error al eliminar el jugador");
            return null;
        }
    }

    public bool DeleteAll() {
        try {
            _context.Jugador.RemoveRange(_context.Jugador);
            _context.SaveChanges();
            return true;
        } catch (Exception e) {
            _logger.Error(e,"Error al eliminar toda la base de datos");
            return false;
        }
    }

    public Result<Jugador, DomainError> Restore(int id) {
        try {
            var entity = _context.Jugador.Find(id);
            if (entity == null) 
                return Result.Failure<Jugador, DomainError>(JugadorErrors.NotFound(id));
            entity.IsDeleted = false;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            _logger.Information("Jugador con {Id} fue restaurado correctamente", id);
            return Result.Success<Jugador, DomainError>(entity.ToModel()!);
        }
        catch (Exception e) {
            _logger.Error(e,"No se pudo restaurar jugador con id: {Id}", id);
            return Result.Failure<Jugador, DomainError>(JugadorErrors.DataBaseError(e.Message));

        }
    }

    public Jugador? GetByPlayerName(string name) {
        try {
            var entity = _context.Jugador.AsNoTracking().FirstOrDefault(j => j.PlayerName == name);
            return entity?.ToModel();

        } catch (Exception e) {
            _logger.Error(e, "Error al buscar jugador con combre: {Name}", name);
            return null;
        }
    }

    public IEnumerable<Jugador>? GetByRol(string rol) {
        try {
            var entities = _context.Jugador
                .Where(j => j.Position.ToString().ToLower() == rol.ToLower() && !j.IsDeleted)
                .ToList();
            return entities.Select(j => j.ToModel());

        } catch (Exception e) {
            _logger.Error(e, "Error al buscar jugadores con rol: {Rol}", rol);
            return null;
        }
    }

    public IEnumerable<Jugador>? GetByTeam(string team) {
        try {
            var entities = _context.Jugador
                .Where(j => j.Team.ToString().ToLower() == team.ToLower() && !j.IsDeleted)
                .ToList();
            return entities.Select(j => j.ToModel());
        } catch (Exception e) {
            _logger.Error(e, "Error al buscar jugadores con equipo: {Team}", team);
            return null;
        }
        
    }

    public bool ExisteJugador(string jugador) {
        try {
            return _context.Jugador.Any(n => n.PlayerName == jugador);
        } catch (Exception e) { 
            _logger.Error(e,"Error al verificar el nombre");
            return false;
        }
    }

    public (Jugador? jugador1, Jugador? jugador2) CompararJugadores(int id1, int id2) {
        var j1 = GetById(id1);
        var j2 = GetById(id2);
        return (j1, j2);
    }
}