using ProjetoDeEstagio2;

namespace ProjetoWeb.Models
{
    public class AlunoTeste
    {        
        Aluno aluno = new();

        public int Matricula => aluno.Matricula;

        public string Nome => aluno.Nome;

        public EnumeradorSexo Sexo => aluno.Sexo;

        public DateTime Nascimento => aluno.Nascimento;

        public string CPF => aluno.CPF;

    }
}