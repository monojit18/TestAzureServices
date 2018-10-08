using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TestAzureServices.Views.CustomControls;
using TestAzureServices.iOS.CustomRenderers;

[assembly:ExportRenderer(typeof(CircleImageView), typeof(CircleImagerenderer))]
namespace TestAzureServices.iOS.CustomRenderers
{
 
    public class CircleImagerenderer : ImageRenderer
    {


        private void CreateCircle()
        {
            double min = Math.Min(Element.Width, Element.Height);
            Control.Layer.CornerRadius = (float)(min / 2.0);
            Control.Layer.MasksToBounds = false;
            Control.Layer.BorderColor = Color.Transparent.ToCGColor();
            Control.Layer.BorderWidth = 3;
            Control.ClipsToBounds = true;
           
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            CreateCircle();

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                CreateCircle();
            }
        }


    }
}
