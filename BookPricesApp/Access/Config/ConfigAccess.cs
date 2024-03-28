using BookPricesApp.Domain.Config;

namespace BookPricesApp.Access.Config;
internal class ConfigAccess : IConfigAccess
{
    private AppConfig _config = new();
}
