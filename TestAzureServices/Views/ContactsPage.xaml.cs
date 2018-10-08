using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TestAzureServices.Views.CustomControls;
using TestAzureServices.ViewModels;

namespace TestAzureServices.Views
{
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();

            //var titleControl = new TitleUserControl();
            //titleControl.TitleText = "Contacts Collection";
            //titleControl.LoaderVisible = true;
            //titleControl.ImageSource = "red_dot.png";

            //NavigationPage.SetTitleView(this, titleControl);

            BindingContext = new ContactsViewModel();

        }
    }
}
