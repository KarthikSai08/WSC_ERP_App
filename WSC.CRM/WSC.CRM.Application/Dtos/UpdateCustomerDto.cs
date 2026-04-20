namespace WSC.CRM.Application.Dtos
{
    public sealed record UpdateCustomerDto(int CxId, string? CxName, string? CxEmail,
                                            string? CxPhone, string? Street,
                                            string? City, string? State,
                                            string? ZipCode, string? Country);
}
