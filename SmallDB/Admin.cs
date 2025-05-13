using System.Text.Json.Serialization;

namespace SmallDB
{
    public class Admin
    {
        [JsonPropertyName("username")]
        private string username { get;}
        [JsonPropertyName("password")]
        private string password { get; }

        public Admin() { }
        public string getName()
        {
            return username;
        }
        public string getPassword()
        { 
            return password;
        }
    }
}
