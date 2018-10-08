using System;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Plugin.Connectivity;
using Subsystems.CustomSQliteORM.External;

namespace TestAzureServices.ViewModels
{
    public class PostsViewModel : ViewModeBase
    {

        private bool _isBusy;
        private CMPSqliteORMProxy _sqliteProxy;

        private PostModel DummyPost()
        {

            return (new PostModel()
            {

                PostString = $"POST:{DateTime.UtcNow.Second.ToString()}",
                DateTimeString = DateTime.UtcNow.ToString()

            });

        }


        private async Task FetchPostsAsync()
        {


            if (IsBusy == true)
                return;
                

            using (var httpClient = new HttpClient())
            {

                
                httpClient.BaseAddress = new Uri("https://easypostsapp.azurewebsites.net/");

                var httpResponse = await httpClient.GetAsync($"api/posts/");
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                var postModelsList = JsonConvert.DeserializeObject<List<PostModel>>(responseString);

                Posts.Clear();
                foreach (var postModel in postModelsList)
                {

                    if (string.IsNullOrEmpty(postModel.DateTimeString) == true)
                        postModel.DateTimeString = DateTime.UtcNow.ToString();

                    Posts.Add(postModel);

                }

                IsBusy = false;



            }

        }

        private bool PreparePostDB()
        {

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPathString = Path.Combine(folderPath + "/postdb.sql");
            _sqliteProxy = new CMPSqliteORMProxy(dbPathString);

            var result = _sqliteProxy.Create<PostModel>();
            return result;

        }

        private async Task<bool> AddPostLocalAsync(PostModel postModel)
        {

            var result = await _sqliteProxy.InsertAsync(postModel);
            return result;

        }

        private async Task<List<PostModel>> FetchPostLocalAsync()
        {

            var result = await _sqliteProxy.FetchAsync<PostModel>();
            return result;

        }

        private async Task<bool> PerformAddAsync(PostModel postModel)
        {

            using (var httpClient = new HttpClient())
            {


                httpClient.BaseAddress = new Uri("https://easypostsapp.azurewebsites.net/");

                var postModelString = JsonConvert.SerializeObject(postModel);
                var httpContent = new StringContent(postModelString, Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PutAsync($"api/posts/", httpContent);
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return ((httpResponse.StatusCode == HttpStatusCode.OK)
                        || (httpResponse.StatusCode == HttpStatusCode.Created));


            }

        }

        private async Task<bool> AddPostsAsync()
        {

            var postModel = DummyPost();
            var couldAdd = await AddPostLocalAsync(postModel);
            if (couldAdd == false)
                return false;

            if (CrossConnectivity.Current.IsConnected == false)
                return false;

            couldAdd = await PerformAddAsync(postModel);
            return couldAdd;

        }

        public ObservableCollection<PostModel> Posts { get; set; }
        public Command LoadPostsCommand { get; set; }
        public Command AddPostCommand { get; set; }

        public bool IsBusy
        {

            get
            {

                return _isBusy;

            }
            set
            {

                _isBusy = value;
                OnPropertyChanged("IsBusy");

            }


        }

        public PostsViewModel()
        {

            Posts = new ObservableCollection<PostModel>();
            LoadPostsCommand = new Command(async () => await FetchPostsAsync());

            AddPostCommand = new Command(async () => await AddPostsAsync());
            _isBusy = false;

            PreparePostDB();



            //var imageBytesArray = SharedAppInitializer.SharedInstance.GetImageBytes("fruit3.jpeg");
            //UploadImageAsync("fruit3", "jpeg", imageBytesArray);
            //FetchContactsAsync();

        }
    }
}
