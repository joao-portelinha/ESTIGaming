using ESTIGamingAPI.Data;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Games.Any()) 
            {
                var games = new List<Game>()
                {
                    new Game()
                    {
                        Name = "God of War",
                        Price = 49.99M,
                        Description = "Teste",
                        Rating = 9.4M,
                        ImageURL = "https://assets.reedpopcdn.com/god-of-war-walkthrough-guide-5004-1524154731667.jpg/BROK/resize/1200x1200%3E/format/jpg/quality/70/god-of-war-walkthrough-guide-5004-1524154731667.jpg",
                        Genre = new Genre()
                        {
                            Name = "Ação"
                        },
                        Platform = new Platform()
                        {
                            Name = "PC"
                        }
                    }
                };
                dataContext.Games.AddRange(games);
                dataContext.SaveChanges();
            }

            if (!dataContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        Role= new Role()
                        {
                            Name = "Administrador"
                        },
                        Name = "João Portelinha",
                        Email = "20481@stu.ipbeja.pt",
                        Password = "Teste123"
                    }
                };
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            };
        }
    }
}
