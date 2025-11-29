using System.Collections.Generic;
using AV2.Domain.Entities;
namespace AV2.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Cliente ObterPorId(int id);
        IEnumerable<Cliente> ObterTodos();
        void Adicionar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Remover(int id);
    }
}
