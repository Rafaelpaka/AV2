using System;
using System.Threading;
using System.Threading.Tasks;

namespace AV2.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IProdutoRepository Produtos { get; }
        ICarrinhoRepository Carrinhos { get; }
        IPedidoRepository Pedidos { get; }
        IPagamentoRepository Pagamentos { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        int Commit();
        void Rollback();
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}