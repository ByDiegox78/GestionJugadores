using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestionEsports.Esports.Models;

namespace GestionEsports.Entity;


[Table("Jugadores")]
public class JugadorEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public Equipo Team { get; set; }

    [Required]
    [MaxLength(100)]
    public string PlayerName { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
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

    [Required]
    [Column(TypeName = "TEXT")]
    public Pais Country { get; set; }

    [Required]
    [MaxLength(1)]
    public string FlashKeybind { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
