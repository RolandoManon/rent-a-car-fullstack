using RentACarAPI.Models;
using System.Text.Json.Serialization;

public class Renta
{
    public int Id { get; set; }

    public int VehiculoId { get; set; }

    [JsonIgnore]
    public Vehiculo? Vehiculo { get; set; }

    public string ClienteNombre { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public decimal Total { get; set; }
}
