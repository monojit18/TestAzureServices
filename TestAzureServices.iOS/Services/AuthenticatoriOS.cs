using System;
using System.Linq;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TestAzureServices.Services;

namespace TestAzureServices.iOS.Services
{

    public class AuthenticatoriOS : IAuthenticator
    {

        private AuthenticatorService _authenticatorService;

        public AuthenticatoriOS()
        {

            var platformParameters = new PlatformParameters(UIApplication
                                                            .SharedApplication
                                                            .KeyWindow.RootViewController,
                                                            false, PromptBehavior.Auto);
            _authenticatorService = new AuthenticatorService(platformParameters);

        }

        public async Task<string> AuthenticateAsync()
        {


            var tokenString = await _authenticatorService.AuthenticateAsync();
            return tokenString;

        }

    }
}
