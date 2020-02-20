using Microsoft.AspNetCore.Authentication;

namespace Authetication
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        //To make Realm option mandatory implement IPostConfigureOptions<TOptions>
        public string Realm { get; set; }
    }
}