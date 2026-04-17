using Microsoft.AspNetCore.Mvc;
using RentACarAPI.Models;
using RentACarAPI.Data;

namespace RentACarAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vehiculos = _context.Vehiculos.ToList();
            return Ok(vehiculos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Vehiculo vehiculo)
        {
            if (vehiculo == null)
            {
                return BadRequest("Datos inválidos");
            }

            _context.Vehiculos.Add(vehiculo);
            _context.SaveChanges();

            return Ok(vehiculo);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);

            if (vehiculo == null)
            {
                return NotFound("Vehiculo no encontrado");
            }
            return Ok(vehiculo);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Vehiculo vehiculo)
        {
            var vehiculoExistente = _context.Vehiculos.Find(id);

            if (vehiculoExistente == null)
            {
                return NotFound("Vehiculo no encontrado");
            }

            vehiculoExistente.Marca = vehiculo.Marca;
            vehiculoExistente.Modelo = vehiculo.Modelo;
            vehiculoExistente.Anio = vehiculo.Anio;
            vehiculoExistente.PrecioPorDia = vehiculo.PrecioPorDia;
            vehiculoExistente.Disponible = vehiculo.Disponible;
            vehiculoExistente.ImagenUrl = vehiculo.ImagenUrl;
            vehiculoExistente.ImagenesExtra = vehiculo.ImagenesExtra;

            _context.SaveChanges();

            return Ok(vehiculoExistente);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);

            if (vehiculo == null)
            {
                return NotFound("Vehiculo no encontrado");
            }

            _context.Vehiculos.Remove(vehiculo);
            _context.SaveChanges();
            return Ok("Vehiculo Eliminado");
        }

        [HttpPut("devolver/{id}")]
        public IActionResult DevolverVehiculo(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);

            if (vehiculo == null)
                return NotFound();

            vehiculo.Disponible = true;
            _context.SaveChanges();

            return Ok(vehiculo);
        }

    }
}