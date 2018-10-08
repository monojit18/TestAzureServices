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
    public class ImageAnalysisViewModel
    {

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

        public ImageAnalysisViewModel()
        {

            var imageBytesArray = SharedAppInitializer.SharedInstance.GetImageBytes("fruit3.jpeg");
            UploadImageAsync(imageBytesArray);

        }
    }
}
