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
                    },

                    new Game()
                    {
                        Name = "Call of Duty: Modern Warfare II",
                        Price = 69.99M,
                        Description = "Call of Duty: Modern Warfare II é um jogo de tiro em primeira pessoa desenvolvido pela Infinity Ward e publicado pela Activision. É uma sequência direta do reboot de 2019 e é o 19º jogo da franquia Call of Duty.",
                        Rating = 8.2M,
                        ImageURL = "https://meups.com.br/wp-content/uploads/2022/11/modern-warfare-ii-4-1-900x503.jpg",
                        Genre = new Genre()
                        {
                            Name = "Shooter"
                        },
                        Platform = new Platform()
                        {
                            Name = "PS5"
                        }
                    },

                    new Game()
                    {
                        Name = "Pokémon Scarlet",
                        Price = 59.99M,
                        Description = "Pokémon Scarlet e Pokémon Violet são dois jogos eletrônicos de RPG desenvolvidos pela Game Freak e publicados pela Nintendo e The Pokémon Company. Anunciados em fevereiro de 2022, são os primeiros títulos da nona geração da série de jogos Pokémon. Foram lançados em 18 de novembro de 2022 para Nintendo Switch.",
                        Rating = 8.9M,
                        ImageURL = "https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_656/b_white/f_auto/q_auto/ncom/pt_BR/games/switch/p/pokemon-scarlet-switch/hero",
                        Genre = new Genre()
                        {
                            Name = "RPG"
                        },
                        Platform = new Platform()
                        {
                            Name = "Nintendo Switch"
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
                        Name = "Admin",
                        Email = "admin@admin.com",
                        Password = "admin"
                    },

                    new User()
                    {
                        Role= new Role()
                        {
                            Name = "Utilizador"
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
