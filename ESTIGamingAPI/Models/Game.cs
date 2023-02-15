namespace ESTIGamingAPI.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public string ImageURL { get; set; }
        public Genre Genre { get; set; }
        public Platform Platform { get; set; }
    }
}
