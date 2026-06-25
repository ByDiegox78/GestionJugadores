using CSharpFunctionalExtensions;
using GestionEsports.Esports.Error;
using GestionEsports.Esports.Error.Common;
using GestionEsports.Esports.Models;

namespace GestionEsports.Esports.Validator;

public class JugadorValidator : IValidator<Jugador> {
    public Result<Jugador, DomainError> Validar(Jugador entity) {
        var errores = new List<string>();

        if (!Enum.IsDefined(entity.Team)) {
            errores.Add("El equipo no coincide con los disponibles");
        }
        if (!Enum.IsDefined(entity.Position)) {
            errores.Add("La posicion no coincide con los disponibles");
        }
        if (!Enum.IsDefined(entity.Country)) {
            errores.Add("El pais no coincide con los disponibles");
        }
        if (string.IsNullOrWhiteSpace(entity.PlayerName) || entity.PlayerName.Length < 3) {
            errores.Add("La nommbre debe contener al manos 3 carazteres");
        }
        if (entity.Games <= 0)
            errores.Add("Games debe ser mayor a 0");
        if (entity.WinRate < 0 || entity.WinRate > 1)
            errores.Add("WinRate debe estar entre 0 y 1");
        if (entity.KDA < 0)
            errores.Add("KDA no puede ser negativo");
        if (entity.AvgKills < 0 || entity.AvgDeaths < 0 || entity.AvgAssists < 0)
            errores.Add("Los promedios no pueden ser negativos");
        if (entity.CSPerMin < 0 || entity.GoldPerMin < 0)
            errores.Add("CSPerMin y GoldPerMin no pueden ser negativos");
        if (entity.KPPercent < 0 || entity.KPPercent > 1)
            errores.Add("KP% debe estar entre 0 y 1");
        if (entity.DamagePercent < 0 || entity.DamagePercent > 1)
            errores.Add("DamagePercent debe estar entre 0 y 1");
        if (entity.PentaKills < 0 || entity.SoloKills < 0)
            errores.Add("Las kills no pueden ser negativas");
        if (entity.FlashKeybind != "D" && entity.FlashKeybind != "F")
            errores.Add("FlashKeybind debe ser D o F");
        if (errores.Any())
            return Result.Failure<Jugador, DomainError>(JugadorErrors.Validation(errores));
        return Result.Success<Jugador, DomainError>(entity);
    }
}