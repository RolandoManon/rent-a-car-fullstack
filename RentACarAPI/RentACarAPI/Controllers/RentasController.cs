using Microsoft.AspNetCore.Mvc;
using RentACarAPI.Data;
using RentACarAPI.Models;

namespace RentACarAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CrearRenta([FromBody] Renta renta)
        {
            var vehiculo = _context.Vehiculos.Find(renta.VehiculoId);

            if (vehiculo == null)
            {
                return NotFound("Vehículo no existe");
            }

            if (!vehiculo.Disponible)
            {
                return BadRequest("Vehículo no disponible");
            }

            // Calcular días
            var dias = (renta.FechaFin - renta.FechaInicio).Days;

            if (dias <= 0)
            {
                return BadRequest("Fechas inválidas");
            }

            // Calcular total
            renta.Total = dias * vehiculo.PrecioPorDia;

            // Marcar como no disponible
            vehiculo.Disponible = false;

            _context.Rentas.Add(renta);
            _context.SaveChanges();

            return Ok(new
            {
                renta.Id,
                renta.ClienteNombre,
                renta.FechaInicio,
                renta.FechaFin,
                renta.Total,
                Vehiculo = new
                {
                    vehiculo.Id,
                    vehiculo.Marca,
                    vehiculo.Modelo,
                    vehiculo.PrecioPorDia
                }
            });
        }

        [HttpPut("devolver/{id}")]
        public IActionResult DevolverVehiculo(int id)
        {
            var renta = _context.Rentas.Find(id);

            if (renta == null)
                return NotFound("Renta no encontrada");

            var vehiculo = _context.Vehiculos.Find(renta.VehiculoId);

            if (vehiculo == null)
                return NotFound("Vehículo no encontrado");

            vehiculo.Disponible = true;

            _context.SaveChanges();

            return Ok("Vehículo devuelto correctamente");
        }
    }
}