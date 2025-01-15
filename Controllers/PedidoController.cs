using BtgItDerivApiPedidos.Services;
using Microsoft.AspNetCore.Mvc;

namespace BtgItDerivApiPedidos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("{codigoCliente}/{codigoPedido}/valor-total")]
        public async Task<IActionResult> GetValorTotalPedido(int codigoCliente, int codigoPedido)
        {
            var valorTotal = await _pedidoService.GetValorTotalPedidoAsync(codigoCliente, codigoPedido);
            if (valorTotal == 0)
            {
                return NotFound(new { message = $"Nenhum pedido encontrado para o codigoCliente: {codigoCliente} e codigoPedido: {codigoPedido}." });
            }
            return Ok(valorTotal);
        }

        [HttpGet("cliente/{codigoCliente}/quantidade")]
        public async Task<IActionResult> GetQuantidadePedidosPorCliente(int codigoCliente)
        {
            var quantidade = await _pedidoService.GetQuantidadePedidosPorClienteAsync(codigoCliente);
            if (quantidade == 0)
            {
                return NotFound(new { message = $"Nenhum pedido encontrado para o codigoCliente: {codigoCliente}." });
            }
            return Ok(quantidade);
        }

        [HttpGet("cliente/{codigoCliente}/pedidos")]
        public async Task<IActionResult> GetPedidosPorCliente(int codigoCliente)
        {
            var pedidos = await _pedidoService.GetPedidosPorClienteAsync(codigoCliente);
            if (pedidos == null || !pedidos.Any())
            {
                return NotFound(new { message = $"Nenhum pedido encontrado para o codigoCliente: {codigoCliente}." });
            }
            return Ok(pedidos);
        }
    }
}