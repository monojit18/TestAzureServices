using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;
using TestAzureServices.Commons;
using TestAzureServices.Services;

namespace TestAzureServices.ViewModels
{

    public delegate void AuthenticationCallback(object sender, AuthenticationEventArgs eventArgs);

    public class AuthenticationEventArgs : EventArgs
    {

        public bool isAuthenticated { get; set; }

    }

    public class AuthenticationViewModel : ViewModeBase
    {

        private async Task AuthenticateAsync()
        {

            //AuthenticationCallback.Invoke(this, new AuthenticationEventArgs()
            //{

            //    isAuthenticated = true

            //});

            using (var scope = SharedAppInitializer.SharedInstance.Container.BeginLifetimeScope())
            {


                var authenticator = scope.ResolveOptional<IAuthenticator>();
                var accessToken = await authenticator.AuthenticateAsync();
                Console.WriteLine(accessToken);

                var eventArgs = new AuthenticationEventArgs()
                {

                    isAuthenticated = !(string.IsNullOrEmpty(accessToken))
                };

                AuthenticationCallback.Invoke(this, eventArgs);

            }

        }

        public ICommand AuthenticationCommand { get; set; }
        public event AuthenticationCallback AuthenticationCallback;

        public AuthenticationViewModel()
        {

            AuthenticationCommand = new Command(async () => await AuthenticateAsync());

        }
    }
}
