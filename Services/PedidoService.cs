using BtgItDerivApiPedidos.Models;
using BtgItDerivApiPedidos.Data;
using MongoDB.Driver;

namespace BtgItDerivApiPedidos.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoService(PedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<decimal> GetValorTotalPedidoAsync(int codigoCliente, int codigoPedido)
        {
            var pedido = await _pedidoRepository.GetPedidoByCodigoAsync(codigoCliente, codigoPedido);
            return pedido?.ValorPedido ?? 0;
        }

        public async Task<long> GetQuantidadePedidosPorClienteAsync(int codigoCliente)
        {
            return await _pedidoRepository.GetQuantidadePedidosPorClienteAsync(codigoCliente);
        }

        public async Task<List<Pedido>> GetPedidosPorClienteAsync(int codigoCliente)
        {
            return await _pedidoRepository.GetPedidosPorClienteAsync(codigoCliente);
        }
    }
}