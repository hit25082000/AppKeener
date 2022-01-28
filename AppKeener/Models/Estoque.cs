using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppKeener.Models
{
    public class Estoque : Entity
    {
        [DisplayName("Produto")]
        public Guid ProdutoId { get; set; }

        [DisplayName("Usuario")]
        [DataType(DataType.EmailAddress)]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Recebidos")]
        [Range(0, int.MaxValue, ErrorMessage = "Apenas numeros positivos")]
        public int Recebido { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Enviados")]
        [Range(0, int.MaxValue, ErrorMessage = "Apenas numeros positivos")]
        public int Enviado { get; set; }

        [DisplayName("Data de Movimentação")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataCadastro { get; set; }

        /* EF Relations */
        public Produto Produto { get; set; }
    }
}
