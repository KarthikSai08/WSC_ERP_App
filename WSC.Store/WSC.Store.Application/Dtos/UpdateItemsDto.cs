namespace WSC.Store.Application.Dtos
{
    public class UpdateItemsDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
