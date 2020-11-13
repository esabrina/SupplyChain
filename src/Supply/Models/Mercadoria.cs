using System;
using System.ComponentModel.DataAnnotations;


namespace Supply.Models
{
    public class Mercadoria
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Fabricante { get; set; }
    }
}
