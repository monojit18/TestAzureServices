using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;
using TestAzureServices.Commons;
using TestAzureServices.Services;

namespace TestAzureServices.ViewModels
{

    public delegate void ShowCapturedImage(ImageSource imageSource);

    public class ImageAnalysisViewModel
    {

        private ICapturePhotoService _imageService;

        private async Task UploadImageAsync(byte[] imageBytesArray)
        {

            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://eastus.api.cognitive.microsoft.com/vision/v1.0/");
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                                                     "dd12e60258d1424e862185c88bd4411a");

                using(var byteArrayContent = new ByteArrayContent(imageBytesArray, 0,
                                                                  imageBytesArray.Length))
                {

                    byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    var httpResponse = await httpClient.PostAsync("analyze?visualFeatures=Categories,Tags,Description", byteArrayContent);
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);

                }

            }

        }

        private async Task CapturePhotoAsync()
        {

            _imageService = DependencyService.Get<ICapturePhotoService>();
            var capturedBytes = await _imageService.CapturePhotoAsync();
            var imageSource = ImageSource.FromStream(() =>
            {

                return (new MemoryStream(capturedBytes));

            });

            ShowCapturedImage.Invoke(imageSource);

        }

        public ShowCapturedImage ShowCapturedImage { get; set; }
        public ICommand CaptureImageCommand { get; set; }

        public ImageAnalysisViewModel()
        {


            CaptureImageCommand = new Command(async () => await CapturePhotoAsync());


            //var imageBytesArray = SharedAppInitializer.SharedInstance.GetImageBytes("fruit3.jpeg");
            //UploadImageAsync(imageBytesArray);

        }
    }
}
