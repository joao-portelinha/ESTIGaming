namespace ESTIGamingAPI.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public int GenreId { get; set; }
        public int PlatformId { get; set; }
        public string ImageURL { get; set; }
    }
}
