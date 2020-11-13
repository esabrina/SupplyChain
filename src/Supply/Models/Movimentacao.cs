using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supply.Models
{
    public class Movimentacao
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Data/Hora")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public DateTime DataHora { get; set; }
        [Display(Name = "Tipo de Movimentação")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public bool MovimentacaoEntrada {get;set;}
        [Required(ErrorMessage = "Campo obrigatório.")]
        public int Quantidade { get; set; }
        public string Local { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Mercadoria")]
        public int IdMercadoria { get; set; }
        [ForeignKey(nameof(IdMercadoria))]
        public Mercadoria Mercadoria { get; set; }
    }
}
