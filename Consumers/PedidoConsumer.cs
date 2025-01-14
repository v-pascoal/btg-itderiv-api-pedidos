using MassTransit;
using System.Text.Json;
using BtgItDerivApiPedidos.Models;
using BtgItDerivApiPedidos.Data;
using BtgItDerivApiPedidos.Helpers.Validators;

namespace BtgItDerivApiPedidos.Consumers
{
    public class PedidoConsumer : IConsumer<Pedido>
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly ClienteRepository _clienteRepository;

        public PedidoConsumer(PedidoRepository pedidoRepository, ClienteRepository clienteRepository)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task Consume(ConsumeContext<Pedido> context)
        {
            try
            {
                // Desserializar a mensagem
                var message = context.Message;
                Console.WriteLine("Mensagem serializada: {0}", JsonSerializer.Serialize(message));

                if (!PedidoValidator.IsValid(message))
                {
                    Console.WriteLine("Mensagem fora do padrão - Descartada.");
                    // Registrar um log detalhado da mensagem inválida
                    Console.WriteLine("Mensagem inválida: {0}", JsonSerializer.Serialize(message));
                    return; // Rejeita a mensagem sem lançar exceção
                }

                Console.WriteLine("Mensagem recebida com sucesso.");

                var pedidoId = await _pedidoRepository.AddOrUpdatePedidoAsync(message);
                Console.WriteLine($"Operação na coleção de pedidos foi feita com sucesso. ID: {pedidoId}");

                var cliente = new Cliente { CodigoCliente = message.CodigoCliente };
                var clienteId = await _clienteRepository.UpsertClienteAsync(cliente);
                Console.WriteLine($"Operação na coleção de clientes foi feita com sucesso. ID: {clienteId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                // Registrar um log detalhado do erro
                Console.WriteLine("Erro ao processar a mensagem: {0}", ex.ToString());
                // Não lança a exceção novamente para evitar erro no MassTransit
            }
        }
    }
}