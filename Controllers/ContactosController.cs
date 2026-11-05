using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftlutionicAPI.Data;
using SoftlutionicAPI.Models;
using SoftlutionicAPI.Services;

namespace SoftlutionicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly SoftlutionicDbContext _context;
        private readonly MongoContactoService _mongoService;
        private readonly ILogger<ContactosController> _logger;

        public ContactosController(
            SoftlutionicDbContext context,
            MongoContactoService mongoService,
            ILogger<ContactosController> logger)
        {
            _context = context;
            _mongoService = mongoService;
            _logger = logger;
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

            // =====================================================
            // 1. REGISTRO PRINCIPAL EN AZURE SQL
            // =====================================================

            contacto.FechaRegistro = DateTime.Now;

            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();

            // =====================================================
            // 2. REGISTRO COMPLEMENTARIO EN MONGODB ATLAS
            // =====================================================

            try
            {
                var solicitudMongo = new SolicitudContactoMongo
                {
                    Nombre = contacto.Nombre,
                    Correo = contacto.Correo,
                    Telefono = contacto.Telefono,
                    Mensaje = contacto.Mensaje ?? string.Empty,

                    Tipo = "contacto_web",
                    Origen = "pagina_web",
                    Estado = "pendiente",
                    Prioridad = "normal",
                    Asunto = "Formulario de contacto",
                    FechaCreacion = DateTime.UtcNow
                };

                await _mongoService.CrearSolicitudAsync(solicitudMongo);
            }
            catch (Exception ex)
            {
                // El contacto ya quedó almacenado en Azure SQL.
                // No se elimina aunque MongoDB presente un error.
                _logger.LogError(
                    ex,
                    "El contacto {ContactoId} fue guardado en Azure SQL, " +
                    "pero no pudo sincronizarse con MongoDB Atlas.",
                    contacto.Id
                );
            }

            return Ok(contacto);
        }
    }
}