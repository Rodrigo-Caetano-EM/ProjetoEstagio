using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoWeb.Models
{
    [Table("Sexos")]
    public class Sexo
    {
        public int CategoriaId { get; set; }

        [DisplayName("Sexo")]
        public string? Nome { get; set; }

        public List<Sexo>? Sexos { get; set; }
    }
}