namespace WSC.CRM.Application.Dtos
{
    public sealed class UpdateCustomerDto
    {
        public int CxId { get; set; }
        public string? CxName { get; set; }
        public string? CxEmail { get; set; }
        public string? CxPhone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}
