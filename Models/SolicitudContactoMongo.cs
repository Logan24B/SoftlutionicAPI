using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SoftlutionicAPI.Models
{
    public class SolicitudContactoMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("tipo")]
        public string Tipo { get; set; } = "contacto_web";

        [BsonElement("origen")]
        public string Origen { get; set; } = "pagina_web";

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("correo")]
        public string Correo { get; set; } = string.Empty;

        [BsonElement("telefono")]
        public string? Telefono { get; set; }

        [BsonElement("asunto")]
        public string? Asunto { get; set; }

        [BsonElement("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [BsonElement("estado")]
        public string Estado { get; set; } = "pendiente";

        [BsonElement("prioridad")]
        public string Prioridad { get; set; } = "normal";

        [BsonElement("asignado_a")]
        public string? AsignadoA { get; set; }

        [BsonElement("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [BsonElement("notificacion")]
        public EstadoNotificacionMongo Notificacion { get; set; } = new();

        [BsonElement("historial")]
        public List<HistorialSolicitudMongo> Historial { get; set; } = new();
    }

    public class EstadoNotificacionMongo
    {
        [BsonElement("generada")]
        public bool Generada { get; set; }

        [BsonElement("leida")]
        public bool Leida { get; set; }

        [BsonElement("fecha_lectura")]
        public DateTime? FechaLectura { get; set; }
    }

    public class HistorialSolicitudMongo
    {
        [BsonElement("estado")]
        public string Estado { get; set; } = "pendiente";

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}