using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TestAzureServices.Views.CustomControls;
using TestAzureServices.ViewModels;

namespace TestAzureServices.Views
{
    public partial class OCRExtractPage : ContentPage
    {
        public OCRExtractPage()
        {
            InitializeComponent();

            var ocrViewModel = new OCRExtractViewModel();
            ocrViewModel.ShowOCRImage = (ImageSource imageSource) =>
            {

                CapturedImage.Source = imageSource;

            };

            BindingContext = ocrViewModel;

        }
    }
}
