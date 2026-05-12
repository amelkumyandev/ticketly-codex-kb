namespace Ticketly.Domain.Entities;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static bool IsSupported(string? role)
    {
        return string.Equals(role, Admin, StringComparison.Ordinal)
            || string.Equals(role, Customer, StringComparison.Ordinal);
    }
}
