using System;
using AV2.Domain.ValueObjects;
using AV2.Domain.Exceptions;

namespace AV2.Domain.Entities
{
    public class Cliente
    {
        public int IdCliente { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public CPF CPF { get; private set; }
        public Endereco Endereco { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }

        private Cliente() 
        { 
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public static Cliente Create(string nome, Email email, CPF cpf, Endereco endereco = null)
        {
            var cliente = new Cliente();
            cliente.AlterarNome(nome);
            cliente.Email = email ?? throw new ArgumentNullException(nameof(email));
            cliente.CPF = cpf ?? throw new ArgumentNullException(nameof(cpf));
            cliente.Endereco = endereco;
            
            return cliente;
        }

        public void AlterarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ValidacaoException(nameof(Nome), "Nome não pode ser vazio.");

            if (nome.Length < 3)
                throw new ValidacaoException(nameof(Nome), "Nome deve ter no mínimo 3 caracteres.");

            if (nome.Length > 200)
                throw new ValidacaoException(nameof(Nome), "Nome deve ter no máximo 200 caracteres.");

            Nome = nome.Trim();
        }

        public void AlterarEmail(Email novoEmail)
        {
            Email = novoEmail ?? throw new ArgumentNullException(nameof(novoEmail));
        }

        public void AlterarEndereco(Endereco novoEndereco)
        {
            Endereco = novoEndereco ?? throw new ArgumentNullException(nameof(novoEndereco));
        }

        public void Desativar()
        {
            if (!Ativo)
                throw new OperacaoNaoPermitidaException("Cliente já está desativado.");
            
            Ativo = false;
        }

        public void Reativar()
        {
            if (Ativo)
                throw new OperacaoNaoPermitidaException("Cliente já está ativo.");
            
            Ativo = true;
        }

        public bool PodeRealizarCompras()
        {
            return Ativo;
        }
    }
}