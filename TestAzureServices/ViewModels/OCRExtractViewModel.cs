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

    public delegate void ShowOCRImage(ImageSource imageSource);

    public class OCRExtractViewModel : ViewModeBase
    {
    
        private ICapturePhotoService _imageService;
        private Command _uploadImageCommand;
        private bool _canUpload;

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

        private async Task CapturePhotoAsync()
        {

            _imageService = DependencyService.Get<ICapturePhotoService>();
            var imageBytesArray = await _imageService.CapturePhotoAsync();
            if ((imageBytesArray == null) || (imageBytesArray.Length == 0))
                return;

            CanUpload = true;
            var imageSource = ImageSource.FromStream(() =>
            {

                return (new MemoryStream(imageBytesArray));

            });

            string imageNameString = "image";
            var extensionString = DateTime.UtcNow.Second.ToString();

            ShowOCRImage.Invoke(imageSource);
            UploadImageCommand = new Command(async () => await UploadImageAsync(imageNameString,
                                                                                extensionString,
                                                                                imageBytesArray));

        }

        public ShowOCRImage ShowOCRImage { get; set; }

        public bool CanUpload
        {

            get
            {

                return _canUpload;

            }
            set
            {

                _canUpload = value;
                OnPropertyChanged("CanUpload");

            }


        }

        public Command UploadImageCommand
        {

            get
            {

                return _uploadImageCommand;

            }
            set
            {

                _uploadImageCommand = value;
                OnPropertyChanged("UploadImageCommand");

            }


        }

        public ICommand CaptureImageCommand { get; set; }

        public OCRExtractViewModel()
        {


            CaptureImageCommand = new Command(async () => await CapturePhotoAsync());
            _canUpload = false;

        }
    }

}
