using System;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using TestAzureServices.Services;

namespace TestAzureServices.iOS.Services
{

    public class CaptureServiceOS : ICapturePhotoService
    {

        private UIImagePickerController _imagePicker;
        private TaskCompletionSource<byte[]> _imageTask;

        private void CancelCapture()
        {

            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            rootViewController.DismissViewController(false, () => { });

        }

        private void StartCapture()
        {

            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            rootViewController.PresentViewController(_imagePicker, true, () => { });

        }

        public async Task<byte[]> CapturePhotoAsync()
        {

            _imageTask = new TaskCompletionSource<byte[]>();

            _imagePicker = new UIImagePickerController();
            _imagePicker.PrefersStatusBarHidden();

            _imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;

            _imagePicker.FinishedPickingMedia += OnCaptureComplete;
            _imagePicker.Canceled += OnCaptureCancelled;

            StartCapture();

            var imageBytes = await _imageTask.Task;
            return imageBytes;

        }

        protected void OnCaptureComplete(object sender,
                                         UIImagePickerMediaPickedEventArgs e)
        {
            
            var mediaTypeString = e.Info[UIImagePickerController.MediaType].ToString();
            if (string.Compare(mediaTypeString, "public.image", true) == -1)
                return;

            var referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")]
                                as NSUrl;
            if (referenceURL != null)
                Console.WriteLine("Url:" + referenceURL.ToString());

            UIImage originalImage = e.Info[UIImagePickerController.OriginalImage]
                                     as UIImage;
            if (originalImage != null)
            {


                originalImage = originalImage.Scale(originalImage.Size,
                                                    (nfloat)(originalImage
                                                             .CurrentScale * 0.5));
                var imageBytes = originalImage.AsJPEG().ToArray();
                _imageTask.SetResult(imageBytes);

            }

            CancelCapture();

        }

        void OnCaptureCancelled(object sender, EventArgs e)
        {

            CancelCapture();

        }
    }
}
