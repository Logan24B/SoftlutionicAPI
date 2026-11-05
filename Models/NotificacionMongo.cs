using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SoftlutionicAPI.Models
{
    public class NotificacionMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [BsonElement("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [BsonElement("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [BsonElement("origen")]
        public string Origen { get; set; } = "api_contacto";

        [BsonElement("referencia")]
        public ReferenciaNotificacionMongo Referencia { get; set; } = new();

        [BsonElement("prioridad")]
        public string Prioridad { get; set; } = "normal";

        [BsonElement("estado")]
        public string Estado { get; set; } = "activa";

        [BsonElement("destinatarios_roles")]
        public List<string> DestinatariosRoles { get; set; } =
            new() { "ADMIN", "SUPERUSER" };

        [BsonElement("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [BsonElement("lecturas")]
        public List<LecturaNotificacionMongo> Lecturas { get; set; } = new();
    }

    public class ReferenciaNotificacionMongo
    {
        [BsonElement("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [BsonElement("id_referencia")]
        public string? IdReferencia { get; set; }
    }

    public class LecturaNotificacionMongo
    {
        [BsonElement("usuario_id_sql")]
        public long UsuarioIdSql { get; set; }

        [BsonElement("fecha_lectura")]
        public DateTime FechaLectura { get; set; }
    }
}
