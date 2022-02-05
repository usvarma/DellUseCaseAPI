
namespace UserService.Models
{
    public class Token
    {
        public Token(string bearer)
        {
            Bearer = bearer;
        }

        public string TokenString { get; set; }
        public string Bearer { get; }
    }
}
