
namespace UserService.Models
{
    public class HashedPassword
    {
        public string PasswordHashed { get; set; }
        public string SaltUnhashed { get; set; }

        public HashedPassword(string passwordHashed, string saltUnhashed)
        {
            PasswordHashed = passwordHashed;
            SaltUnhashed = saltUnhashed;
        }
    }
}
