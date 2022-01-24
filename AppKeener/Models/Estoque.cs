using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppKeener.Models
{
    public class Estoque : Entity
    {
        [DisplayName("Produto")]
        public Guid ProdutoId { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Recebidos")]
        public int Recebido { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Enviados")]
        public int Enviado { get; set; }

        [DisplayName("Data de Movimentação")]
        public DateTime DataCadastro { get; set; }

        /* EF Relations */
        public Produto Produto { get; set; }
    }
}
