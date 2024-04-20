using BookPricesApp.Core.Utils;
using RestSharp;

namespace BookPricesApp.Core.Access.Contract;

public static class AmazonAccessError
{
    public static Error NullContect(string code)
    {
        var description = "response.Content was found null";
        return Error.Create(code, description);
    }

    public static Error BadRequest(string code, RestResponse response)
    {
        var message = $"{code}\n{response.ErrorMessage}";
        var description = $"{response.ErrorMessage}\n{response.Content}";
        return Error.Create(message, description);
    }

    public static Error NullAccessToken()
    {
        return Error.Create("_accessToken was found null");
    }
}
