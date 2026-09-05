using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftlutionicAPI.Data;
using SoftlutionicAPI.Models;

namespace SoftlutionicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly SoftlutionicDbContext _context;

        public ContactosController(SoftlutionicDbContext context)
        {
            _context = context;
        }

        // GET: api/contactos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            return await _context.Contactos
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        // POST: api/contactos
        [HttpPost]
        public async Task<ActionResult<Contacto>> PostContacto(Contacto contacto)
        {
            if (string.IsNullOrWhiteSpace(contacto.Nombre) ||
                string.IsNullOrWhiteSpace(contacto.Correo))
            {
                return BadRequest("Nombre y Correo son obligatorios.");
            }

            contacto.FechaRegistro = DateTime.Now;

            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();

            return Ok(contacto);
        }
    }
}