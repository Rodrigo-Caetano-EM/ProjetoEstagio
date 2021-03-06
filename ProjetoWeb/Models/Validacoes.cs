using EMRepository;
using ProjetoDeEstagio2;
using System.Text.RegularExpressions;

namespace ProjetoWeb.Models
{
    public class Validacoes
    {
        public static bool EhCPFValido(string cpf)
        {
            string valor = cpf.Replace(".", "").Replace("-", "");

            if (int.TryParse(valor, out int cpfnumerico))
            {
                return false;
            }

            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
            {
                if (valor[i] != valor[0])
                {
                    igual = false;
                }
            }
            if (igual || valor == "12345678909")
            {
                return false;
            }

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numeros[i] = int.Parse(valor[i].ToString());
            }

            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;



            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    return false;
                }
            }

            else if (numeros[9] != 11 - resultado)
            {
                return false;
            }

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;


            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                {
                    return false;
                }

            }

            else
            {
                if (numeros[10] != 11 - resultado)
                {
                    return false;
                }
            }
            return true;
        }
        public bool EhValido(string nome, DateTime nascimento, string cpf)
        {
            string tamanhoDoAno = nascimento.Year.ToString();
            if (tamanhoDoAno.Length != 4)
            {
                return false;
            }
            else
            {
                if (nascimento.Year <= 1920 || nascimento.Year > Convert.ToInt32(DateTime.Now.AddMonths(-7).Year))
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(cpf))
            {
                if (!EhCPFValido(cpf))
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(nome))
            {
                if (!Regex.IsMatch(nome, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    return false;
                }

                if (nome.Length < 1)
                {
                    return false;
                }
            }
            DateTime dataNascimentoMinima = new(1900, 01, 01, 00, 00, 00);
            DateTime dataDeTesteAluno = Convert.ToDateTime(nascimento);
            DateTime dataAtual = DateTime.Now;
            int CompararDatas = DateTime.Compare(dataNascimentoMinima, dataDeTesteAluno);
            int idade = dataAtual.Year - dataDeTesteAluno.Year;
            if (dataDeTesteAluno.Year >= dataAtual.Year)
            {
                if (CompararDatas < 0 || dataAtual.Year < dataDeTesteAluno.Year)
                {
                    return false;
                }
            }
            int mesNascimento = Convert.ToInt32(dataDeTesteAluno.Month) + 12;
            int mesAtual = Convert.ToInt32(dataAtual.Month) + 12;
            if (Math.Abs(mesAtual - mesNascimento) >= 7 && idade <= 1)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(nome))
            {
                if (!Regex.IsMatch(nome, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    return false;
                }
                if (nome.Length < 1)
                {
                    return false;
                }
            }   
            return true;
        }
        public static bool JaTemEsseCPF(string matricula, string cpf)
        {
            RepositorioAluno repositorioAluno = new();

            if (cpf == String.Empty)
            {
                return false;
            }
            else
            {
                string valor = cpf.Replace(".", "").Replace("-", "");
                IEnumerable<Aluno> alunos = repositorioAluno.Get(alunosCPF => alunosCPF.CPF == cpf && alunosCPF.Matricula != Convert.ToInt32(matricula));
                return alunos.Any();
            }
        }
        public bool JaTemEssaMatricula(int matricula)
        {
            RepositorioAluno repositorioAluno = new();

            string stringMatricula = Convert.ToString(matricula);

            if (stringMatricula == String.Empty)
            {
                return false;
            }
            else
            {
                IEnumerable<Aluno> alunos = repositorioAluno.Get(alunosMatricula => alunosMatricula.Matricula == Convert.ToInt32(matricula));
                return alunos.Any();
            }
        }
    }
}
