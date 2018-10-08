using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TestAzureServices.Commons;

namespace TestAzureServices.Services
{
    public class AuthenticatorService
    {

        private PlatformParameters _platformParameters;

        public AuthenticatorService(PlatformParameters platformParameters)
        {

            _platformParameters = platformParameters;

        }

        public async Task<string> AuthenticateAsync()
        {

            var authContext = new AuthenticationContext(SharedAppInitializer.KAuthorizeString);
            var tokenExists = authContext.TokenCache.ReadItems().Any();
            if (tokenExists)
            {

                var tokenCacheItem = authContext.TokenCache.ReadItems().ToList()[0];
                return tokenCacheItem.AccessToken;

            }


            var authResult = await authContext.AcquireTokenAsync(SharedAppInitializer.KResourceString,
                                                                 SharedAppInitializer.KClientIdString,
                                                                 new Uri(SharedAppInitializer
                                                                         .KRedirectUriString),
                                                                 _platformParameters);
            return authResult.AccessToken;


        }
    }
}
