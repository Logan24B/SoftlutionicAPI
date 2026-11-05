using Microsoft.AspNetCore.Mvc;
using SoftlutionicAPI.Models;
using SoftlutionicAPI.Services;

namespace SoftlutionicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesContactoController : ControllerBase
    {
        private readonly MongoContactoService _mongoService;

        public SolicitudesContactoController(MongoContactoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpPost]
        public async Task<ActionResult<SolicitudContactoMongo>> Crear(
            SolicitudContactoMongo solicitud)
        {
            if (string.IsNullOrWhiteSpace(solicitud.Nombre) ||
                string.IsNullOrWhiteSpace(solicitud.Correo) ||
                string.IsNullOrWhiteSpace(solicitud.Mensaje))
            {
                return BadRequest(
                    "Nombre, correo y mensaje son obligatorios.");
            }

            solicitud.Tipo = "contacto_web";
            solicitud.Origen = "pagina_web";
            solicitud.Estado = "pendiente";

            var creada = await _mongoService.CrearSolicitudAsync(solicitud);

            return CreatedAtAction(
                nameof(Crear),
                new { id = creada.Id },
                creada);
        }
    }
}