using BtgItDerivApiPedidos.Models;
using MongoDB.Driver;

namespace BtgItDerivApiPedidos.Data
{
    public class ClienteRepository
    {
        private readonly IMongoCollection<Cliente> _clientesCollection;

        public ClienteRepository(IMongoDatabase database)
        {
            _clientesCollection = database.GetCollection<Cliente>("clientes");
        }

        public async Task<string> UpsertClienteAsync(Cliente cliente)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.CodigoCliente, cliente.CodigoCliente);

            var update = Builders<Cliente>.Update
                .Set(c => c.CodigoCliente, cliente.CodigoCliente);

            var options = new FindOneAndUpdateOptions<Cliente>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = await _clientesCollection.FindOneAndUpdateAsync(filter, update, options);

            return result.Id;
        }
    }
}