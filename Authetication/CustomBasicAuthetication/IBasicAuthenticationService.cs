using System.Threading.Tasks;

namespace Authetication
{
    public interface IBasicAuthenticationService
    {
        Task<bool> IsValidUserAsync(string user, string password);
    }
}