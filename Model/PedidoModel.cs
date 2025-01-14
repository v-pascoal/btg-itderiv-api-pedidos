using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtgItDerivApiPedidos.Models
{
    public class Pedido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        public int CodigoPedido { get; set; }
        public int CodigoCliente { get; set; }
        public List<Item> Itens { get; set; }

        //Ao invés de calcular em cada requisição, optei por já salvar na base com o valor.
        //Dado que é o processo resultante de uma fila, não tem tanto impacto de percepção de performance.
        public decimal ValorPedido { get; set; } 
    }

    public class Item
    {
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}