using System.ComponentModel;

namespace ESTIGamingWebsite.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [DisplayName("Nome")]
        public string Name { get; set; }
    }
}
