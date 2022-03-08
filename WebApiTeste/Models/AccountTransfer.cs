using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiTeste.Models
{
    // classe de conta que contem todos os atributos
    public class AccountTransfer
    {
        //usei um Guid para gerar um ID para transferência, ele é gerado toda vez que uma operação de transferência é iniciada
        public Guid Id { get; set; }

        //número da conta de origem
        public string AccountOrigin { get; set; }

        //número da conta de destino
        public string AccountDestination { get; set; }

        //valor a ser transferido
        public double Value { get; set; }

        //Enumerator de status, separei em arquivo para segregar melhor as funções 
        public StatusInfo Status { get; set; }

        //String que armazenará as mensagêns de erro
        public string StatusMsg { get; set; }
    }
}