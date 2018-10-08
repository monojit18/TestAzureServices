using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using System.Windows.Input;
using Autofac;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TestAzureServices.Commons;
using TestAzureServices.Services;
using TestAzureServices.Droid.Services;

namespace TestAzureServices.Droid
{
    [Activity(Label = "TestAzureServices", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges =
              ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            var builder = new ContainerBuilder();
            builder.RegisterType<AuthenticatorDroid>().As<IAuthenticator>();
            SharedAppInitializer.SharedInstance.Container = builder.Build();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
                                                    requestCode, resultCode, data);
        }
    }
}