using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI2025.ViewModel
{
    public class LoginViewModel
    {
        public LoginViewModel() { }
        public LoginViewModel(string username, string password, bool remember = false)
        {
            Username = username;
            Password = password;
            RememberMe = remember;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
