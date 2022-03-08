using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiTeste.Models
{
    // classe de conta que contem todos os atributos
    public class AccountTransfer
    {
        //usei um Guid para gerar um ID para transfer�ncia, ele � gerado toda vez que uma opera��o de transfer�ncia � iniciada
        public Guid Id { get; set; }

        //n�mero da conta de origem
        public string AccountOrigin { get; set; }

        //n�mero da conta de destino
        public string AccountDestination { get; set; }

        //valor a ser transferido
        public double Value { get; set; }

        //Enumerator de status, separei em arquivo para segregar melhor as fun��es 
        public StatusInfo Status { get; set; }

        //String que armazenar� as mensag�ns de erro
        public string StatusMsg { get; set; }
    }
}