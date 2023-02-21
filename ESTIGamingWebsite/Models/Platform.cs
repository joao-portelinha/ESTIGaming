using System.ComponentModel;

namespace ESTIGamingWebsite.Models
{
    public class Platform
    {
        public int Id { get; set; }
        [DisplayName("Nome")]
        public string Name { get; set; }
    }
}
