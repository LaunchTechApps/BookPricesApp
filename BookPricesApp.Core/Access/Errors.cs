using BookPricesApp.Core.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Access;
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

public static class EBayAccessError
{
    public static Error RateLimit => Error.Create("EbayAccess", "Rate limit exceeded");
    public static Error EmptyApiKeys => Error.Create("EbayAccess", "api key list is empty");
    public static Error RantOutOfKeys => Error.Create("EbayAccess", "ran out of api keys");
}

