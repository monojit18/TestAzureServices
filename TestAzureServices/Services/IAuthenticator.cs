using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TestAzureServices.Services
{
    public interface IAuthenticator
    {

        Task<string> AuthenticateAsync();
        

    }
}
