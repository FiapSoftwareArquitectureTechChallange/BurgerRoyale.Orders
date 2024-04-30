using System.ComponentModel;

namespace BurgerRoyale.Orders.Domain.Enumerators;

public enum OrderStatus
{
    [Description("Pagamento pendente")]
    PagamentoPendente,

    [Description("Pagamento reprovado")]
    PagamentoReprovado,

    [Description("Em preparação")]
    EmPreparacao,

    [Description("Pronto")]
    Pronto,

    [Description("Finalizado")]
    Finalizado
}