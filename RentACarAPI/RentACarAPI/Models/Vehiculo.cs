namespace RentACarAPI.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Marca {  get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public decimal PrecioPorDia { get; set; }
        public bool Disponible { get; set; }
        public string ImagenUrl { get; set; } 
        public string ImagenesExtra { get; set; }
    }
}
