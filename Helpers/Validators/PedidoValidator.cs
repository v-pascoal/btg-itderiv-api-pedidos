using BtgItDerivApiPedidos.Models;

namespace BtgItDerivApiPedidos.Helpers.Validators
{
    public static class PedidoValidator
    {
        public static bool IsValid(Pedido pedido)
        {
            if (pedido == null ||
                pedido.CodigoPedido <= 0 ||
                pedido.CodigoCliente <= 0 ||
                pedido.Itens == null || pedido.Itens.Count == 0)
            {
                return false;
            }

            foreach (var item in pedido.Itens)
            {
                if (string.IsNullOrWhiteSpace(item.Produto) ||
                    item.Quantidade <= 0 ||
                    item.Preco < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}