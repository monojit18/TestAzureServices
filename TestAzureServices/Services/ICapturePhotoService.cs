using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestAzureServices.Services
{
    public interface ICapturePhotoService
    {

        Task<byte[]> CapturePhotoAsync();

    }
}
