using Newtonsoft.Json;

namespace Actio.Common.Commands
{
    /// 
    public class CreateUser : ICommand
    {
        [JsonProperty(PropertyName ="email")]
        public string Email {get;set; }
        [JsonProperty(PropertyName = "password")]
        public string Password {get;set; }
        [JsonProperty(PropertyName = "name")]
        public string Name {get;set;}
    }
}