namespace WSC.Delivery.Domain.Enums
{
    public enum DeliveryStatus
    {
        Pending = 0,
        Assigned = 1,
        PickedUp = 2,
        InTransit = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Failed = 6,
        Cancelled = 7
    }
}
