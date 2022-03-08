namespace WebApiTeste.Models
{
    // Utilizei um enum para representar os status de cada transação
    public enum StatusInfo
    {
        InQueue = 0,
        Processing = 1,
        Confirmed = 2,
        Error = 3
    }
}
