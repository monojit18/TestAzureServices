using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TestAzureServices.ViewModels;

namespace TestAzureServices.Views
{
    public partial class AuthenticationPage : ContentPage
    {

        private void PrepareViewModel()
        {

            var authViewModel = new AuthenticationViewModel();
            authViewModel.AuthenticationCallback += async (object sender,
                                                            AuthenticationEventArgs
                                                           eventArgs) =>
            {

                Console.WriteLine(eventArgs.isAuthenticated);
                if (eventArgs.isAuthenticated == true)
                    await Navigation.PushAsync(new HomePage());


            };

            BindingContext = authViewModel;
            NavigationPage.SetHasNavigationBar(this, false);


        }

        public AuthenticationPage()
        {
            InitializeComponent();
            PrepareViewModel();
        }
    }
}
