using System;
using System.Collections.Generic;
using AV2.Domain.Entities;
using AV2.Domain.Interfaces;
using AV2.Domain.ValueObjects;
using AV2.Application.DTOs.ClienteDTOs;

namespace AV2.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public ClienteResponseDTO CriarCliente(ClienteCreateDTO dto)
        {
            // Criar Value Objects necessários
            var email = Email.Create(dto.Email);
            var cpf = CPF.Create(dto.CPF);
            
            // Usar o método factory estático
            var cliente = Cliente.Create(dto.Nome, email, cpf);

            _clienteRepo.Adicionar(cliente);

            return new ClienteResponseDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome
            };
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