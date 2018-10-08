using System;
using System.ComponentModel;

namespace TestAzureServices.ViewModels
{
    public class ViewModeBase : INotifyPropertyChanged
    {
    

        protected void OnPropertyChanged(string propertyNameString)
        {

            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyNameString));

        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
