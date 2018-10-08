using System;
using Newtonsoft.Json;

namespace TestAzureServices.Models
{
    public class EasyBlobModel
    {

        [JsonProperty("name")]
        public string BlobNameString { get; set; }

        [JsonProperty("contents")]
        public string BlobContentsString { get; set; }


        public string ExceprtionMessageString { get; set; }

    }
}
