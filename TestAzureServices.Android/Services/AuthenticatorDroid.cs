using System;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Xamarin.Forms;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TestAzureServices.Services;
using TestAzureServices.Commons;

namespace TestAzureServices.Droid.Services
{
    public class AuthenticatorDroid : IAuthenticator
    {

        private AuthenticatorService _authenticatorService;

        public AuthenticatorDroid()
        {

            var platformParameters = new PlatformParameters((Activity)Forms.Context,
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
