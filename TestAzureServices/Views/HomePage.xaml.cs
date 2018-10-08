using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestAzureServices.Views
{
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

        }
    }
}
