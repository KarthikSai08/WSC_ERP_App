namespace WSC.Shared.Contracts.Dtos.CRMLayer
{
    public sealed record CustomerResponseDto
    {
        public int CxId { get; set; }
        public string CxName { get; set; }
        public string CxPhone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
