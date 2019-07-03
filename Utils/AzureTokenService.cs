using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace oauthExample.Utils
{
    public class AzureTokenService : IAzureTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationContext _context;
        private readonly ClientCredential _appCredentials;
        private readonly string _userToken;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _authority;

        /// <summary>
        /// requires that AzureConfig contains Authority, ClientId, ClientSecret
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="cache"></param>
        public AzureTokenService(IOptions<AzureConfig> config, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _clientId = config.Value.ClientId;
            _clientSecret = config.Value.ClientSecret;
            _authority = config.Value.Authority;
            _httpContextAccessor = httpContextAccessor;
            _context = new AuthenticationContext(_authority, new InMemoryTokenCache(GetUserId(), cache));
            var auth = AuthenticationHeaderValue.Parse(httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            _userToken = auth.Parameter;
            _appCredentials = new ClientCredential(_clientId, _clientSecret);
        }


        /// <summary>
        /// gets a token for the application towards resource 
        /// using AzureConfig to get clientid / secret and authority
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<string> GetApplicationToken(string resource)
        {
            AuthenticationResult result = await _context.AcquireTokenAsync(resource, _appCredentials);

            if (result == null)
                throw new Exception("401: Failed to obtain the JWT token");

            return result.AccessToken;
        }

        /// <summary>
        /// gets a token for the application using authority and scope props
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="resource"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public async Task<string> GetApplicationToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            var clientCred = new ClientCredential(_clientId, _clientSecret);
            var result = await authContext.AcquireTokenAsync(resource, clientCred);

            return result.AccessToken;
        }

        /// <summary>
        /// gets a delegate token for logged in user. 
        /// uses AzureConfig for info on authority, clientid and clientsecret
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<string> GetDelegateAccessTokenForResource(string resourceId)
        {
            var result = await _context.AcquireTokenAsync(resourceId, _appCredentials, new UserAssertion(_userToken)).ConfigureAwait(false);
            return result.AccessToken;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}