using System.Text.Json.Serialization;

namespace SmallDB
{
    public class Admin
    {
        [JsonPropertyName("username")]
        public string username { get; set; }
        [JsonPropertyName("password")]
        public string password { get; set; }

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
