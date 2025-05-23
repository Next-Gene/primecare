using System.Runtime.Serialization;

namespace PrimeCare.Core.Entities.Order
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment Received")]

        PaymentReceived,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}
