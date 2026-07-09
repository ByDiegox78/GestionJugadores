namespace GestionEsports.Esports.Models;

public class Jugador {
    public int Id { get; set; }
    public Equipo Team { get; set; }
    public string PlayerName { get; set; }
    public Rol Position { get; set; }
    public int Games { get; set; }
    public double WinRate { get; set; }
    public double KDA { get; set; }
    public double AvgKills { get; set; }
    public double AvgDeaths { get; set; }
    public double AvgAssists { get; set; }
    public double CSPerMin { get; set; }
    public int GoldPerMin { get; set; }
    public double KPPercent { get; set; }
    public double DamagePercent { get; set; }
    public int DPM { get; set; }
    public double VSPM { get; set; }
    public double AvgWPM { get; set; }
    public double AvgWCPM { get; set; }
    public double AvgVWPM { get; set; }
    public int GD15 { get; set; }
    public int CSD15 { get; set; }
    public int XPD15 { get; set; }
    public double FBPercent { get; set; }
    public double FBVictim { get; set; }
    public int PentaKills { get; set; }
    public int SoloKills { get; set; }
    public Pais Country { get; set; }
    public string FlashKeybind { get; set; }
}