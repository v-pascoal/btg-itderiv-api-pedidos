using BtgItDerivApiPedidos.Models;
using MongoDB.Driver;

namespace BtgItDerivApiPedidos.Data
{
    public class PedidoRepository
    {
        private readonly IMongoCollection<Pedido> _pedidosCollection;

        public PedidoRepository(IMongoDatabase database)
        {
            _pedidosCollection = database.GetCollection<Pedido>("pedidos");
        }

        public async Task<string> AddOrUpdatePedidoAsync(Pedido pedido)
        {
            // Calcular o valor total do pedido
            pedido.ValorPedido = pedido.Itens.Sum(item => item.Quantidade * item.Preco);

            var filter = Builders<Pedido>.Filter.And(
                Builders<Pedido>.Filter.Eq(p => p.CodigoPedido, pedido.CodigoPedido),
                Builders<Pedido>.Filter.Eq(p => p.CodigoCliente, pedido.CodigoCliente)
            );

            var update = Builders<Pedido>.Update
                .Set(p => p.CodigoPedido, pedido.CodigoPedido)
                .Set(p => p.CodigoCliente, pedido.CodigoCliente)
                .Set(p => p.Itens, pedido.Itens)
                .Set(p => p.ValorPedido, pedido.ValorPedido);

            var options = new FindOneAndUpdateOptions<Pedido>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = await _pedidosCollection.FindOneAndUpdateAsync(filter, update, options);

            return result.Id;
        }

        public async Task<Pedido> GetPedidoByCodigoAsync(int codigoCliente, int codigoPedido)
        {
            var filter = Builders<Pedido>.Filter.And(
                Builders<Pedido>.Filter.Eq(p => p.CodigoCliente, codigoCliente),
                Builders<Pedido>.Filter.Eq(p => p.CodigoPedido, codigoPedido)
            );
            return await _pedidosCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> GetQuantidadePedidosPorClienteAsync(int codigoCliente)
        {
            return await _pedidosCollection.CountDocumentsAsync(p => p.CodigoCliente == codigoCliente);
        }

        public async Task<List<Pedido>> GetPedidosPorClienteAsync(int codigoCliente)
        {
            return await _pedidosCollection.Find(p => p.CodigoCliente == codigoCliente).ToListAsync();
        }
    }
}