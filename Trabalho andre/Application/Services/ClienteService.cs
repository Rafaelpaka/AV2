using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public Cliente CriarCliente(string nome)
        {
            var cliente = new Cliente { Nome = nome };

            _clienteRepo.Adicionar(cliente);
            return cliente;
        }

        public Cliente ObterPorId(int id)
        {
            return _clienteRepo.ObterPorId(id);
        }

        public IEnumerable<Cliente> ObterTodos()
        {
            return _clienteRepo.ObterTodos();
        }

        public void Atualizar(Cliente cliente)
        {
            _clienteRepo.Atualizar(cliente);
        }

        public void Remover(int id)
        {
            _clienteRepo.Remover(id);
        }
    }
}
