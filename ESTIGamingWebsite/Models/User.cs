using System.ComponentModel;

namespace ESTIGamingWebsite.Models
{
    public class User
    {
        public int Id { get; set; }
        [DisplayName("Nome de Utilizador")]
        public string Name { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Palavra Passe")]
        public string Password { get; set; }
        [DisplayName("Tipo de utilizador")]
        public int RoleId { get; set; }
    }
}
