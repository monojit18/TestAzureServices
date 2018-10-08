using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TestAzureServices.Views.CustomControls;
using TestAzureServices.ViewModels;

namespace TestAzureServices.Views
{
    public partial class PostsPage : ContentPage
    {

        private PostsViewModel _postsViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if ((_postsViewModel.Posts == null) || (_postsViewModel.Posts.Count == 0))
                _postsViewModel.LoadPostsCommand.Execute(null);
        }

        public PostsPage()
        {
            InitializeComponent();

            //var titleControl = new TitleUserControl();
            //titleControl.TitleText = "Contacts Collection";
            //titleControl.LoaderVisible = true;
            //titleControl.ImageSource = "red_dot.png";

            //NavigationPage.SetTitleView(this, titleControl);

            _postsViewModel = new PostsViewModel();
            BindingContext = _postsViewModel;

        }


    }
}
