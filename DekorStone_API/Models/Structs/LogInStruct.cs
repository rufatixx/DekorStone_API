using System;
namespace DekorStone_API.Models.Structs
{
    public class LogInStruct
    {
        public int status { get; set; }
       public string userToken { get; set; }
        public string requestToken { get; set; }
        public int userType { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
