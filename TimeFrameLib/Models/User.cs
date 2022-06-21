namespace TimeFrameLib.Models
{
    public class User
    {
        public int Id { get; set; } //Unique Identifier, non editable
        public string Email { get; set; } //email of user, unique, editable
        public string PasswordHash { get; set; } // hashed pw only
        public string FirstName { get; set; } // editable
        public string LastName { get; set; } //editable
    }
}