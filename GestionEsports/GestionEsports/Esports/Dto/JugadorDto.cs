namespace GestionEsports.Esports.Dto;

public record JugadorDto(
    string Team,
    string PlayerName,
    string Position, 
    int Games,
    string WinRate,
    string KDA,
    string AvgKills,
    string AvgDeaths,
    string AvgAssists,
    string CSPerMin,
    int GoldPerMin,
    string KPPercent,
    int DPM,
    string VSPM,
    string AvgWPM,
    string AvgWCPM,
    string AvgVWPM,
    int GD15,
    int CSD15,
    int XPD15,
    string FBPercent,
    string FBVictim,
    int PentaKills,
    int SoloKills,
    string Country
);