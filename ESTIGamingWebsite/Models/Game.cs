using System.ComponentModel;

namespace ESTIGamingWebsite.Models
{
    public class Game
    {
        public int Id { get; set; }
        [DisplayName("Nome")]
        public string Name { get; set; }
        [DisplayName("Preço")]
        public decimal Price { get; set; }
        [DisplayName("Descrição")]
        public string Description { get; set; }
        [DisplayName("Avaliação")]
        public decimal Rating { get; set; }
        [DisplayName("Genero")]
        public int GenreId { get; set; }
        [DisplayName("Plataforma")]
        public int PlatformId { get; set; }
        public string ImageURL { get; set; }
    }
}
