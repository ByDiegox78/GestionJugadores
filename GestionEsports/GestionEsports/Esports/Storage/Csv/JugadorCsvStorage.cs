using System.Text;
using CSharpFunctionalExtensions;
using GestionEsports.Esports.Dto;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Mapper;
using GestionEsports.Esports.Models;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GestionEsports.Esports.Storage.Csv;

public class JugadorCsvStorage : IStorageJugadorCsv {
    private readonly ILogger _logger = Log.ForContext<JugadorCsvStorage>();

    public Result<bool, DomainError> Salvar(IEnumerable<Jugador> items, string path) {
        try {
            _logger.Debug("Guardando los datos del archivo {Path}", path);
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine(
                "Id;Team;PlayerName;Position;Games;WinRate;KDA;AvgKills;AvgDeaths;AvgAssists;CSPerMin;GoldPerMin;KPPercent;DPM;VSPM;AvgWPM;AvgWCPM;AvgVWPM;GD15;CSD15;XPD15;FBPercent;FBVictim;PentaKills;SoloKills;Country;IsDeleted;CreatedAt;UpdatedAt");
            foreach (var jugador in items) {
                var dto = jugador.ToDto();
                writer.WriteLine(
                    $"{dto.Id};{dto.Team};{dto.PlayerName};{dto.Position};{dto.Games};{dto.WinRate};{dto.KDA};{dto.AvgKills};{dto.AvgDeaths};{dto.AvgAssists};{dto.CSPerMin};{dto.GoldPerMin};{dto.KPPercent};{dto.DPM};{dto.VSPM};{dto.AvgWPM};{dto.AvgWCPM};{dto.AvgVWPM};{dto.GD15};{dto.CSD15};{dto.XPD15};{dto.FBPercent};{dto.FBVictim};{dto.PentaKills};{dto.SoloKills};{dto.Country};{dto.IsDeleted};{dto.CreatedAt};{dto.UpdatedAt}");
            }

            return Result.Success<bool, DomainError>(true);
        } catch (Exception e) {
            _logger.Error(e, "Error al guardar el archivo {Path}", path);
            return Result.Failure<bool, DomainError>(StorageErrors.WriteErrir(e.Message));
        }
    }
    public Result<IEnumerable<Jugador>, DomainError> Cargar(string path) {
        _logger.Debug("Cargando los datos del archivo {Path}", path);
        if (!Path.Exists(path))
            return Result.Failure<IEnumerable<Jugador>, DomainError>(StorageErrors.FileNotFount(path));
        try {
            var jugadores = File.ReadLines(path, Encoding.UTF8)
                .Skip(1)
                .Select(p => p.Split(';'))
                .Select(p => new JugadorDto(
                    p[0],
                    p[1],
                    p[2],
                    p[3],
                    int.Parse(p[4]),
                    p[5],
                    p[6],
                    p[7],
                    p[8],
                    p[9],
                    p[10],
                    int.Parse(p[11]),
                    p[12],
                    int.Parse(p[13]),
                    p[14],
                    p[15],
                    p[16],
                    p[17],
                    int.Parse(p[18]),
                    int.Parse(p[19]),
                    int.Parse(p[20]),
                    p[21],
                    p[22],
                    int.Parse(p[23]),
                    int.Parse(p[24]),
                    p[25],
                    bool.Parse(p[26]),
                    p[27],
                    p[28]
                ).ToModel()).ToList();
            return Result.Success<IEnumerable<Jugador>, DomainError>(jugadores);
        } catch (Exception e) {
            _logger.Error("Error al cargar el archivo {Path}", path);
            return Result.Failure<IEnumerable<Jugador>, DomainError>(StorageErrors.ReadError(e.Message));
        }
    }

}