namespace WSC.CRM.Application.Dtos
{
    public sealed record CreateCustomerDto(string CxName, string CxEmail,
                                            string? CxPhone, string? Street,
                                            string? City, string? State,
                                            string? ZipCode, string? Country);
}
