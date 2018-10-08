using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Graphics;
using Android.Runtime;
using Autofac;
using Xamarin.Forms;
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

            //var capturePermissionInfo = CheckSelfPermission(Manifest.Permission.Camera);
            //if (capturePermissionInfo == Permission.Denied)
            //    RequestPermissions(new string[] { Manifest.Permission.Camera }, 2);
            //else
                //DependencyService.Register<CaptureServiceDroid>();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
                                                        [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode != 2)
                return;

            if (grantResults[0] != Permission.Granted)
                return;

            DependencyService.Register<CaptureServiceDroid>();

        }

        protected override void OnActivityResult(int requestCode, Result resultCode,
                                                 Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {

                var bitmap = data.Extras.Get("data") as Bitmap;
                TaskCompletionSource.SetResult(bitmap);

            }
            else
            {

                AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
                                                        requestCode, resultCode, data);


            }

        }

        public TaskCompletionSource<Bitmap> TaskCompletionSource { get; set; }


    }
}