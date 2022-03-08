using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApiTeste.Models;

namespace WebApiTeste.Controllers
{
    [ApiController]
    [Route("")]
    public class AccountTransferController : ControllerBase
    {

        public static List<AccountTransfer> transactions = new List<AccountTransfer>();


        // get que puxa os dados de contas, a minha API faz a chamada da outra API que retorna as contas
        // utilizei métodos de retorno async para não travar a aplicação durante a execução de um request

        [HttpGet] //verbo do request
        [Route("/accounts")] //endpoint/route
        public async Task<IActionResult> CheckAllAccounts()
        {
            var client = new RestClient("https://acessoaccount.herokuapp.com/api/Account/");
            RestRequest restRequest = new RestRequest("", Method.Get);
            var response = await client.ExecuteAsync(restRequest);

            return Ok(response.Content);
        }

        // Post que transfere fundos
        [HttpPost]
        [Route("/fund-transfer")]
        public async Task<IActionResult> FundTransfer([FromBody] AccountTransfer accountTransfer)
        {
            /* Aqui utilizei alguns if's para tentar tratar os principais erros que podem ocorrer
                1 n° da conta de origem errado
                2 n° da conta de destino errado
                3 o valor a ser transferido acima do saldo
               
            Utilizei a biblioteca do RestSharp para fazer as chamadas entre API's utilizando um RestClient

            Try catch para pegar algum possível erro não listado dentro dos If's

            O plano foi utilizar os métodos "Credit" e "Debit" já existentes na API chamada para fazer a operação de transação */


            try
            {
                accountTransfer.Id = Guid.NewGuid();

                // request de debit na conta de origem (testar saldo, retornar erro caso ocorra)
                var client = new RestClient("https://acessoaccount.herokuapp.com/api/Account/");

                // body json com os dados para a requisição
                var postDebitData = new
                {
                    accountNumber = accountTransfer.AccountOrigin,
                    value = accountTransfer.Value,
                    type = "Debit"
                };

                // request "debit" na conta de destino
                RestRequest restDebitRequest = new RestRequest("", Method.Post).AddJsonBody(postDebitData);
                var responseDebit = await client.ExecuteAsync(restDebitRequest);

                if (responseDebit.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    accountTransfer.Status = StatusInfo.Error;
                    accountTransfer.StatusMsg = "Invalid origin account number";
                    transactions.Add(accountTransfer);
                    return NotFound(new { transactionId = accountTransfer.Id });
                }

                if (responseDebit.Content == "\"Not enough balance\"")
                {
                    accountTransfer.Status = StatusInfo.Error;
                    accountTransfer.StatusMsg = "Not enough balance";
                    transactions.Add(accountTransfer);
                    return BadRequest(new { transactionId = accountTransfer.Id });
                }

                // passando da primeira etapa o processing é setado na variável status
                accountTransfer.Status = StatusInfo.Processing;

                // request "credit" na conta de destino
                var postCreditData = new
                {
                    accountNumber = accountTransfer.AccountDestination,
                    value = accountTransfer.Value,
                    type = "Credit"
                };

                RestRequest restCreditRequest = new RestRequest("", Method.Post).AddJsonBody(postCreditData);
                var responseCredit = await client.ExecuteAsync(restCreditRequest);

                if (responseCredit.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    accountTransfer.Status = StatusInfo.Error;
                    accountTransfer.StatusMsg = "Invalid destination account number";
                    transactions.Add(accountTransfer);
                    return NotFound(new { transactionId = accountTransfer.Id });
                }

                // Finalizando sem erro, status "confirmaded", a transação é armazenada na lista transactions, e o ID é retornado na API
                accountTransfer.Status = StatusInfo.Confirmed;
                transactions.Add(accountTransfer);
                return Ok(new { transactionId = accountTransfer.Id });
            }
            catch (Exception)
            {
                return BadRequest(new { transactionId = accountTransfer.Id });
                throw;
            }

        }

        // get que verifica a transação
        [HttpGet]
        [Route("/fund-transfer/{transactionId}")]
        public IActionResult GetTransferStatus(string transactionId)
        {
            // Aqui é feito um parse para converter o atributo STRING vindo da rota para utiliza-lo como GUID ao pesquisar a listagem de transações
            var wantedId = Guid.Parse(transactionId);

            // A variável search armazena os resultados da pesquisa pelo ID retornando a do elemento na lista de transações
            int search = transactions.IndexOf(transactions.Where(p => p.Id == wantedId).FirstOrDefault());

            // Aqui eu armazeno o status da transação para utiliza-lo na composição do retorno
            var transactionStatus = transactions[search].Status;


            /* conforme a orientação do retorno da verificação aqui montei 2x retornos possíveis, um com 
            a mensagem de erro e outro sem a mensagem de erro, no caso do status ser "confirmed" */

            if (transactionStatus != StatusInfo.Confirmed)
                return Ok(new{Status = Enum.GetName(typeof(StatusInfo), transactionStatus), Message = transactions[search].StatusMsg});
            else
                return Ok(new{Status = Enum.GetName(typeof(StatusInfo), transactionStatus),});
        }

    }

}
