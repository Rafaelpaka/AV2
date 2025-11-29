using System.Collections.Generic;
using AV2.Domain.Entities;
namespace AV2.Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Pagamento ObterPorId(int idPagamento);
        IEnumerable<Pagamento> ObterPorPedido(int idPedido);
        void Registrar(Pagamento pagamento);
        void AtualizarStatus(int idPagamento, string status);
    }
}
