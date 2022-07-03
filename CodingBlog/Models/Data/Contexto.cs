namespace CodingBlog.Models.Data
{
    public class Contexto : IContexto
    {
        public List<Categoria> Categorias { get; set; }
        public List<Post> Posts { get; set; }

        public Contexto()
        {
            var posts = new List<Post>()
            {
                new Post(
                    1,
                    "Tratando Exceptions com C# 9",
                    @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                    ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                    Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                    posuere, laoreet sapien...",
                    "img1.jpg",
                    "michael",
                    "C#,Exceptions",
                    1,
                    new DateTime(2022,2,1)
                ),
                new Post(
                    2,
                    "Usuando AWS Parameter Store para ASP.NET Core Data Protection Keys",
                    @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                    ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                    Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                    posuere, laoreet sapien...",
                    "img1.jpg",
                    "michael",
                    "AWS,DevOps",
                    2,
                    new DateTime(2022,1,1)
                ),new Post(
                    3,
                    "Hello World com MVC 6",
                    @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                    ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                    Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                    posuere, laoreet sapien.<p>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                    ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                    Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                    posuere, laoreet sapien.<p>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                    ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                    Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                    posuere, laoreet sapien.<p>",
                    "img1.jpg",
                    "michael",
                    "C#, Asp.Net, MVC",
                    3,
                    new DateTime(2018,4,10)
                )
            }; 

            Categorias = new List<Categoria>
            {
                new Categoria(1, "C#", posts.Where(e => e.CategoriaId == 1).ToList()),
                new Categoria(2, "AWS", posts.Where(e => e.CategoriaId == 2).ToList()),
                new Categoria(3, "Asp.Net", posts.Where(e => e.CategoriaId == 3).ToList()),
            };
            Posts = posts;
        }
    }
}
