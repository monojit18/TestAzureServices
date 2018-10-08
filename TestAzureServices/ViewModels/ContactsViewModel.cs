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
using Newtonsoft.Json;
using TestAzureServices.Commons;
using TestAzureServices.Services;
using TestAzureServices.Models;

namespace TestAzureServices.ViewModels
{
    public class ContactsViewModel
    {

        private async Task FetchContactsAsync()
        {

            using (var httpClient = new HttpClient())
            {


                httpClient.BaseAddress = new Uri("https://easypostsapp.azurewebsites.net/");

                var httpResponse = await httpClient.GetAsync($"api/posts/");
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);



            }

        }

        private async Task UploadImageAsync(string imageNameString, string extensionString,
                                            byte[] imageBytesArray)
        {

            using (var httpClient = new HttpClient())
            {

                var fileNameString = string.Concat(imageNameString, ".", extensionString);
                var imgae64String = Convert.ToBase64String(imageBytesArray);
                var easyBlobModel = new EasyBlobModel()
                {

                    BlobNameString = fileNameString,
                    BlobContentsString = imgae64String

                };
                var easyBloModelString = JsonConvert.SerializeObject(easyBlobModel);

                httpClient.BaseAddress = new Uri("https://easymobileapp.azurewebsites.net/");

                using (var stringContent = new StringContent(easyBloModelString, Encoding.UTF8))
                {

                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var httpResponse = await httpClient.PutAsync($"api/blob/{imageNameString}/{extensionString}", stringContent);
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);

                }

            }

        }

        public ContactsViewModel()
        {

            //var imageBytesArray = SharedAppInitializer.SharedInstance.GetImageBytes("fruit3.jpeg");
            //UploadImageAsync("fruit3", "jpeg", imageBytesArray);
            //FetchContactsAsync();

        }
    }
}
