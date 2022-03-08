
<h2>Walkthrough Resolution</h2>



1° - Iniciei o projeto no Visual Studio Community e precisei relembrar como era o funcionamento de uma API: Requests e responses, http.

2° - Crier o arquivo "AccountTransfer.cs" que é a classe principal, contendo atributos que permitem retornar e receber um json no formato disposto pelo objeto;

3° - Parei pra estudar protocolo HTTP, verbos REST e API Rest, e cheguei a conclusão de que precisaria de um client REST como RestShart para realizar as chamadas para a outra API

5° - Comecei a "codar" o controller principal "AccountTransferController" que será responsável pelas chamadas e o código presente nos métodos de endpoints
* 5.1 - O 1° método (CheckAllAccounts()) faz uma chamada na API que traz as informações das contas, encapsulei esse método pra minha API.
* 5.2 - O 2° FundTransfer() fica responsável pela função principal de realizar a transferência através das chamada POST da outra API (Debit e Credit), o fluxo desse é importante para retornar diversos resultados e ao final
* 5.3 - O 3° GetTransferStatus() é o método que ao passarmos o ID pela rota nos retorna o status da transação realizada posteriormente

6° - Chegando nesse ponto eu utilizei o Postman para testar as chamadas na minha API e analisar os erros que foram ocorrendo durante o código, em diversos momentos precisei debugar a aplicação para entender linha a linha qual tinha sido o erro.



<h2>ERROS</h2>


- 1 n° da conta de origem errado
- 2 n° da conta de destino errado
- 3 O valor a ser transferido acima do saldo da conta de origem


<h2>Como consumir a API?</h2>

<h5>[api]/accounts</h5>
<h3>Retorna todas as contas bancárias disponíveis</h3>


<h5>[api]/fund-transfer</h5>
<h3>Faz a operação de transferência bancária</h3>

```
{
    "accountOrigin": "acountOriginNumber",
    "accountDestination": "acountDestinationNumber",
    "value": 0.0
}
```

<h3>Retorno</h3>
```
{
    "transactionId": "1911f9a2-9f2e-11ec-b909-0242ac120002"
}
```

<h5>[api]/fund-transfer/{transactionId}</h5>
<h3>Esse faz a operação de transferência bancária </h3>

