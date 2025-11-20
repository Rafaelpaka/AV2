using System.Collections.Generic;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Pagamento ObterPorId(int idPagamento);
        IEnumerable<Pagamento> ObterPorPedido(int idPedido);
        void Registrar(Pagamento pagamento);
        void AtualizarStatus(int idPagamento, string status);
    }
}
