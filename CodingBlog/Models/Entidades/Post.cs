namespace CodingBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descritivo { get; set; }
        public string? Imagem { get; set; }
        public string? CriadoPor { get; set; }
        public string? Tags { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
        public DateTime CadastradoEm{ get; set; }
        public Post(
            int id, string titulo, string descritivo, 
            string imagem, string criadoPor, string tags,
            int categoriaId, DateTime data)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Descritivo = descritivo;
            this.Imagem = imagem;
            this.CriadoPor = criadoPor;
            this.Tags = tags;
            this.CategoriaId = categoriaId;
            this.CadastradoEm = data;
            this.Categoria = new Categoria();
        }
        
        public Post()
        { 
            this.Categoria = new Categoria();
        }

        public string ObterPedacoDescritivo()
        {
            if (string.IsNullOrEmpty(this.Descritivo)) return string.Empty;

            if (this.Descritivo.Length >= 250)
                return this.Descritivo.Substring(0,250) + "...";

            return this.Descritivo;
        }
    }
}
