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

    public delegate void ShowCapturedImage(ImageSource imageSource);

    public class ImageAnalysisViewModel : ViewModeBase
    {
    
        private ICapturePhotoService _imageService;
        private Command _analyzeImageCommand;
        private bool _canUpload;
        private string _analyzedResult;

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

                    var analyzedModel = JsonConvert.DeserializeObject<AnalyzedModel>(responseString);
                    if (analyzedModel == null)
                        return;

                    var captions = analyzedModel.Description.Captions;
                    if (captions == null || captions.Count == 0)
                        return;

                    AnalyzedResult = captions[0].Text;

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
            AnalyzeImageCommand = new Command(async () => await UploadImageAsync(imageBytesArray));

            var imageSource = ImageSource.FromStream(() =>
            {

                return (new MemoryStream(imageBytesArray));

            });

            ShowCapturedImage.Invoke(imageSource);

        }

        public ShowCapturedImage ShowCapturedImage { get; set; }

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

        public string AnalyzedResult
        {

            get
            {

                return _analyzedResult;

            }
            set
            {

                _analyzedResult = value;
                OnPropertyChanged("AnalyzedResult");

            }


        }

        public Command AnalyzeImageCommand
        {

            get
            {

                return _analyzeImageCommand;

            }
            set
            {

                _analyzeImageCommand = value;
                OnPropertyChanged("AnalyzeImageCommand");

            }


        }

        public ICommand CaptureImageCommand { get; set; }

        public ImageAnalysisViewModel()
        {


            CaptureImageCommand = new Command(async () => await CapturePhotoAsync());
            _canUpload = false;
            AnalyzedResult = "Wait....";
        }
    }
}
