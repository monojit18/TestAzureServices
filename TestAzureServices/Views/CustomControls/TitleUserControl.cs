using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TestAzureServices.Views.CustomControls
{
    public partial class TitleUserControl : ContentView
    {

        private static void OnImageSourceChanged(BindableObject bindable, object oldValue,
                                                 object newValue)
        {

            var imageUserControl = (TitleUserControl)bindable;
            imageUserControl.TitleImageView.Source = newValue.ToString();

        }

        private static void OnTitleTextChanged(BindableObject bindable, object oldValue,
                                                 object newValue)
        {

            var imageUserControl = (TitleUserControl)bindable;
            imageUserControl.TitleLabel.Text = newValue.ToString();

        }

        private static void OnLoaderVisibleChanged(BindableObject bindable, object oldValue,
                                                    object newValue)
        {

            var imageUserControl = (TitleUserControl)bindable;
            imageUserControl.LoadingIndicator.IsVisible = (bool)newValue;

        }

        private static void OnLoaderRunningChanged(BindableObject bindable, object oldValue,
                                                    object newValue)
        {

            var imageUserControl = (TitleUserControl)bindable;
            imageUserControl.LoadingIndicator.IsRunning = (bool)newValue;

        }


        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create("ImageSource", typeof(string),
                                    typeof(TitleUserControl),
                                    propertyChanged: OnImageSourceChanged);

        public static readonly BindableProperty TitleTextProperty =
            BindableProperty.Create("TitleText", typeof(string),
                                    typeof(TitleUserControl),
                                    propertyChanged: OnTitleTextChanged);

        public static readonly BindableProperty LoaderVisibleProperty =
            BindableProperty.Create("LoaderVisible", typeof(bool),
                                    typeof(TitleUserControl),
                                    propertyChanged: OnLoaderVisibleChanged);

        public static readonly BindableProperty LoaderRunningProperty =
            BindableProperty.Create("LoaderRunning", typeof(bool),
                                    typeof(TitleUserControl),
                                    propertyChanged: OnLoaderRunningChanged);


        public string ImageSource
        {


            get
            {

                return GetValue(ImageSourceProperty) as string;

            }

            set
            {

                SetValue(ImageSourceProperty, value);

            }

        }

        public string TitleText
        {


            get
            {

                return GetValue(TitleTextProperty) as string;

            }

            set
            {

                SetValue(TitleTextProperty, value);

            }

        }

        public bool LoaderVisible
        {


            get
            {

                return (bool)GetValue(LoaderVisibleProperty);

            }

            set
            {

                SetValue(LoaderVisibleProperty, value);

            }

        }

        public bool LoaderRunning
        {


            get
            {

                return (bool)GetValue(LoaderRunningProperty);

            }

            set
            {

                SetValue(LoaderRunningProperty, value);

            }

        }


        public TitleUserControl()
        {
            InitializeComponent();
        }
    }
}
