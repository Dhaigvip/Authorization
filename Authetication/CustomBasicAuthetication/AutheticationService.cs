using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authetication
{
    public class AutheticationService : IBasicAuthenticationService

    {
        public async Task<bool> IsValidUserAsync(string user, string password)
        {
            return true;
        }
    }
}
