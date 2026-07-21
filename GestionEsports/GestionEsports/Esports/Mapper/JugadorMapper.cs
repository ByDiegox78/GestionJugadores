using System.Globalization;
using GestionEsports.Esports.Dto;
using GestionEsports.Esports.Models;
using GestionEsports.Entity;

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
            model.Country.ToString(),
            model.IsDeleted,
            model.CreatedAt.ToString("O"),
            model.UpdatedAt.ToString("O")
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
            IsDeleted = dto.IsDeleted,
            CreatedAt = DateTime.Parse(dto.CreatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
            UpdatedAt = DateTime.Parse(dto.UpdatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
        };
    }

    public static JugadorEntity ToEntity(this Jugador model) {
        return new JugadorEntity {
            Id = model.Id,
            Team = model.Team,
            PlayerName = model.PlayerName,
            Position = model.Position,
            Games = model.Games,
            WinRate = model.WinRate,
            KDA = model.KDA,
            AvgKills = model.AvgKills,
            AvgDeaths = model.AvgDeaths,
            AvgAssists = model.AvgAssists,
            CSPerMin = model.CSPerMin,
            GoldPerMin = model.GoldPerMin,
            KPPercent = model.KPPercent,
            DamagePercent = model.DamagePercent,
            DPM = model.DPM,
            VSPM = model.VSPM,
            AvgWPM = model.AvgWPM,
            AvgWCPM = model.AvgWCPM,
            AvgVWPM = model.AvgVWPM,
            GD15 = model.GD15,
            CSD15 = model.CSD15,
            XPD15 = model.XPD15,
            FBPercent = model.FBPercent,
            FBVictim = model.FBVictim,
            PentaKills = model.PentaKills,
            SoloKills = model.SoloKills,
            Country = model.Country,
            FlashKeybind = model.FlashKeybind,
            IsDeleted = model.IsDeleted,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static Jugador ToModel(this JugadorEntity entity) {
        return new Jugador {
            Id = entity.Id,
            Team = entity.Team,
            PlayerName = entity.PlayerName,
            Position = entity.Position,
            Games = entity.Games,
            WinRate = entity.WinRate,
            KDA = entity.KDA,
            AvgKills = entity.AvgKills,
            AvgDeaths = entity.AvgDeaths,
            AvgAssists = entity.AvgAssists,
            CSPerMin = entity.CSPerMin,
            GoldPerMin = entity.GoldPerMin,
            KPPercent = entity.KPPercent,
            DamagePercent = entity.DamagePercent,
            DPM = entity.DPM,
            VSPM = entity.VSPM,
            AvgWPM = entity.AvgWPM,
            AvgWCPM = entity.AvgWCPM,
            AvgVWPM = entity.AvgVWPM,
            GD15 = entity.GD15,
            CSD15 = entity.CSD15,
            XPD15 = entity.XPD15,
            FBPercent = entity.FBPercent,
            FBVictim = entity.FBVictim,
            PentaKills = entity.PentaKills,
            SoloKills = entity.SoloKills,
            Country = entity.Country,
            FlashKeybind = entity.FlashKeybind,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}