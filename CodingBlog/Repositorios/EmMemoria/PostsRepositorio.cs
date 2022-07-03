using CodingBlog.Interfaces;
using CodingBlog.Models;

namespace CodingBlog.Repositorios.EmMemoria
{
    public class PostsRepositorio : IPostsRepositorio
    { 
        public PostsRepositorio()
        {
            
        }

        private List<Post> dados = new()
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
                1
            ),
            new Post(
                2,
                "Using AWS Parameter Store for ASP.NET Core Data Protection Keys",
                @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat odio vitae 
                ornare tristique. Vestibulum nulla lectus, venenatis vitae laoreet vel, aliquet quis sem. 
                Praesent placerat finibus leo et laoreet. Curabitur id est interdum, volutpat tortor 
                posuere, laoreet sapien...",
                "img1.jpg",
                "michael",
                "AWS,DevOps",
                2
            )
        };       

        public List<Post> ObterRecentes()
        {
            return dados.OrderByDescending(e => e.CadastradoEm).ToList();
        }

        public List<string> ObterTodasTags()
        {
            var tags = new List<string>();
            List<string> tagsDosPosts = dados.Where(e => !string.IsNullOrEmpty(e.Tags)).Select(e => e.Tags).ToList();
            foreach (var tagsDoPost in tagsDosPosts)
            {
                var tagsArray = tagsDoPost.Split(",").Select(e => e.Trim()).ToList();
                tags.AddRange(tagsArray);
            }
            tags = tags.Distinct().ToList();

            return tags;
        }

        public List<Post> ObterPorTags(string tag)
        {
            return dados.Where(e => e.Tags.Contains(tag)).ToList();
        }

        public List<Post> ObterPorCategoria(int id)
        {
            return dados.Where(e => e.CategoriaId == id).ToList();
        }

        public Post Obter(int id)
        {
            return dados.Single(e => e.Id == id);
        }
    }
}