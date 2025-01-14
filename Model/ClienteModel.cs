using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtgItDerivApiPedidos.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }        
        public int CodigoCliente { get; set; }
    }
}