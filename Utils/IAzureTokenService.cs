using System.Threading.Tasks;

namespace oauthExample.Utils
{
    public interface IAzureTokenService
    {
        Task<string> GetApplicationToken(string resource);
        Task<string> GetApplicationToken(string authority, string resource, string scope);
        Task<string> GetDelegateAccessTokenForResource(string resourceId);
        string GetUserId();
    }
}