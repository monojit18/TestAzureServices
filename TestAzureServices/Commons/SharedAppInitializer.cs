using System;
using Autofac;

#if __IOS__

using UIKit;
using Foundation;
using CoreGraphics;

#endif

namespace TestAzureServices.Commons
{
    public class SharedAppInitializer
    {

        private SharedAppInitializer() { }

        private class Nested
        {

            static Nested() { }
            internal static readonly SharedAppInitializer _instance = new SharedAppInitializer();

        }

        public static SharedAppInitializer SharedInstance
        {
            get
            {
                return Nested._instance;
            }
        }

        public byte[] GetImageBytes(string imageNameString)
        {

            byte[] imageBytesArray = null;

#if __IOS__

            var image = UIImage.FromFile(imageNameString);
            imageBytesArray = image.AsJPEG().ToArray();

#else

#endif

            return imageBytesArray;

        }


        public IContainer Container { get; set; }
        public const string KAuthorizeString = "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47";
        public const string KResourceString = "https://graph.windows.net";
        public const string KClientIdString = "c5a58959-69b4-46e2-8b58-b03c16b885db";
        public const string KRedirectUriString = "http://localhost/XamFormApp";


    }
}
