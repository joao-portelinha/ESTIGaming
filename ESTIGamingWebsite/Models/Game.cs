using System.ComponentModel.DataAnnotations;

namespace ESTIGamingWebsite.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Preço")]
        public decimal Price { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Avaliação")]
        public decimal Rating { get; set; }
        public string ImageURL { get; set; }
    }
}
