using MassTransit;
using System.Text.Json;
using BtgItDerivApiPedidos.Models;

namespace BtgItDerivApiPedidos.Consumers
{
    public class PedidoConsumer : IConsumer<Pedido>
    {
        public async Task Consume(ConsumeContext<Pedido> context)
        {
            var message = context.Message;
            Console.WriteLine(" [x] Received {0}", JsonSerializer.Serialize(message));

            if (message != null && message.Itens != null && message.Itens.Count > 0)
            {
                Console.WriteLine("Mensagem recebida com sucesso.");
            }
            else
            {
                Console.WriteLine("Mensagem fora do padrão - Descartada.");
            }

            await Task.CompletedTask;
        }
    }
}