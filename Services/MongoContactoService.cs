using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SoftlutionicAPI.Configuration;
using SoftlutionicAPI.Models;

namespace SoftlutionicAPI.Services
{
    public class MongoContactoService
    {
        private readonly ContactPushService _push;
        private readonly IMongoCollection<SolicitudContactoMongo> _solicitudes;
        private readonly IMongoCollection<NotificacionMongo> _notificaciones;

        public MongoContactoService(IOptions<MongoDbSettings> settings, ContactPushService push)
        {
            _push = push;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _solicitudes = database.GetCollection<SolicitudContactoMongo>(
                settings.Value.SolicitudesCollection);

            _notificaciones = database.GetCollection<NotificacionMongo>(
                settings.Value.NotificacionesCollection);
        }

        public async Task<SolicitudContactoMongo> CrearSolicitudAsync(
            SolicitudContactoMongo solicitud)
        {
            solicitud.FechaCreacion = DateTime.UtcNow;

            solicitud.Historial.Add(new HistorialSolicitudMongo
            {
                Estado = "pendiente",
                Fecha = solicitud.FechaCreacion
            });

            await _solicitudes.InsertOneAsync(solicitud);

            var notificacion = new NotificacionMongo
            {
                Tipo = "solicitud_contacto",
                Titulo = "Nueva solicitud de contacto",
                Mensaje = $"{solicitud.Nombre} ha enviado una nueva solicitud desde la página web.",
                Origen = "api_contacto",
                Referencia = new ReferenciaNotificacionMongo
                {
                    Tipo = "solicitud_contacto",
                    IdReferencia = solicitud.Id
                },
                Prioridad = solicitud.Prioridad,
                Estado = "activa",
                FechaCreacion = DateTime.UtcNow
            };

            await _notificaciones.InsertOneAsync(notificacion);

            solicitud.Notificacion.Generada = true;

            var filtro = Builders<SolicitudContactoMongo>.Filter
                .Eq(x => x.Id, solicitud.Id);

            var actualizacion = Builders<SolicitudContactoMongo>.Update
                .Set(x => x.Notificacion.Generada, true);

            await _solicitudes.UpdateOneAsync(
                filtro,
                actualizacion
            );

            // Both records are persisted before attempting best-effort push.
            await _push.NotifyAsync(notificacion.Id);

            return solicitud;
        }
    }
}