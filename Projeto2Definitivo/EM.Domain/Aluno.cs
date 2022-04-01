using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDeEstagio2
{
    public class Aluno : IEntidade
    {      
        [Required(ErrorMessage = "A matricula é obrigatória", AllowEmptyStrings = false)]
        [Range(1, 999999999, ErrorMessage = "A matricula não pode: ter mais de 9 digitos, ser 0 ou ser negativa")]
        [Display(Name = "Matricula")]
        public int Matricula { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Nome do aluno")]
        public string Nome { get; set; }

        [StringLength(15, MinimumLength = 11)]
        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [DataType(DataType.Date, ErrorMessage = "erro")]
        [Range(typeof(DateTime), "01/01/1920", "31/12/2021", ErrorMessage = "Idade inválida")]
        public DateTime Nascimento { get; set; }
        [Display(Name = "Sexo")]
        public EnumeradorSexo Sexo { get; set; }

        public Aluno(int matricula, string nome, string cpf, DateTime nascimento, EnumeradorSexo sexo)
        {
            Matricula = matricula;
            Nome = nome;
            CPF = cpf;
            Nascimento = nascimento;
            Sexo = sexo;
        }

        public Aluno() { }

        public override bool Equals(object obj)
        {
            return obj is Aluno aluno &&
                Matricula == aluno.Matricula &&
                Nome == aluno.Nome &&
                CPF == aluno.CPF &&
                Nascimento == aluno.Nascimento &&
                Sexo == aluno.Sexo;
        }

        public override int GetHashCode()
        {
            int hashCode = 2086685141;
            hashCode = hashCode * -1521134295 + Matricula.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nome);
            hashCode = hashCode * -1521134295 + Sexo.GetHashCode();
            hashCode = hashCode * -1521134295 + Nascimento.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CPF);
            return hashCode;
        }
    }
}
