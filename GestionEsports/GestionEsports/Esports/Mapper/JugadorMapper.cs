using System.Globalization;
using GestionEsports.Esports.Dto;
using GestionEsports.Esports.Models;

namespace GestionEsports.Esports.Mapper;

public static class JugadorMapper {
    public static JugadorDto ToDto(this Jugador model) {
        return new JugadorDto(
            model.Id.ToString(),
            model.Team.ToString(),
            model.PlayerName,
            model.Position.ToString(),
            Games: model.Games,
            model.WinRate.ToString(CultureInfo.InvariantCulture),
            KDA: model.KDA.ToString("F2"),
            AvgKills: model.AvgKills.ToString("F1"),
            AvgDeaths: model.AvgDeaths.ToString("F1"),
            AvgAssists: model.AvgAssists.ToString("F1"),
            CSPerMin: model.CSPerMin.ToString("F1"),
            GoldPerMin: model.GoldPerMin,
            KPPercent: $"{model.KPPercent}%",
            DPM: model.DPM,
            VSPM: model.VSPM.ToString("F2"),
            AvgWPM: model.AvgWPM.ToString("F2"),
            AvgWCPM: model.AvgWCPM.ToString("F2"),
            AvgVWPM: model.AvgVWPM.ToString("F2"),
            GD15: model.GD15,
            CSD15: model.CSD15,
            XPD15: model.XPD15,
            model.FBPercent.ToString(CultureInfo.InvariantCulture),
            model.FBVictim.ToString(CultureInfo.InvariantCulture),
            PentaKills: model.PentaKills,
            SoloKills: model.SoloKills,
            model.Country.ToString()
        );
    }

    public static Jugador ToModel(this JugadorDto dto) {
        return new Jugador {
            Id = int.Parse(dto.Id),
            Team = Enum.Parse<Equipo>(dto.Team, true),
            PlayerName = dto.PlayerName,
            Position = Enum.Parse<Rol>(dto.Position, true),
            Games = dto.Games,
            WinRate = double.Parse(dto.WinRate.Replace("%", "")),
            KDA = double.Parse(dto.KDA),
            AvgKills = double.Parse(dto.AvgKills),
            AvgDeaths = double.Parse(dto.AvgDeaths),
            AvgAssists = double.Parse(dto.AvgAssists),
            CSPerMin = double.Parse(dto.CSPerMin),
            GoldPerMin = dto.GoldPerMin,
            KPPercent = double.Parse(dto.KPPercent.Replace("%", "")),
            DPM = dto.DPM,
            VSPM = double.Parse(dto.VSPM),
            AvgWPM = double.Parse(dto.AvgWPM),
            AvgWCPM = double.Parse(dto.AvgWCPM),
            AvgVWPM = double.Parse(dto.AvgVWPM),
            GD15 = dto.GD15,
            CSD15 = dto.CSD15,
            XPD15 = dto.XPD15,
            FBPercent = double.Parse(dto.FBPercent.Replace("%", "")),
            FBVictim = double.Parse(dto.FBVictim.Replace("%", "")),
            PentaKills = dto.PentaKills,
            SoloKills = dto.SoloKills,
            Country = Enum.Parse<Pais>(dto.Country, true),
        };
    }
}