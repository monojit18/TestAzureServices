using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TestAzureServices.Views.CustomControls;
using TestAzureServices.ViewModels;

namespace TestAzureServices.Views
{
    public partial class ImageAnalysisPage : ContentPage
    {
        public ImageAnalysisPage()
        {
            InitializeComponent();

            //var titleControl = new TitleUserControl();
            //titleControl.TitleText = "Image Collection";
            //titleControl.LoaderVisible = true;
            //titleControl.ImageSource = "red_dot.png";

            //NavigationPage.SetTitleView(this, titleControl);

            var imageAnalysisViewModel = new ImageAnalysisViewModel();
            imageAnalysisViewModel.ShowCapturedImage = (ImageSource imageSource) => 
            {

                CapturedImage.Source = imageSource;

            };

            BindingContext = imageAnalysisViewModel;

        }
    }
}
