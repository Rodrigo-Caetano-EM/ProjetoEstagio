using EM.Repository;
using ProjetoDeEstagio2;


namespace ProjetoWeb.Repository
{
    public class AlunoRepository
    {
        RepositorioAluno repositorioAluno = new();

        public void AdicionarAluno(Aluno aluno)
        {
            repositorioAluno.Add(aluno);
        }

        public IEnumerable<Aluno> SelecionarAlunos()
        {
            return repositorioAluno.GetAll();
        }

        public void AtualizarAluno(Aluno aluno)
        {
            repositorioAluno.Update(aluno);
        }
        public void ExcluirAluno(Aluno aluno)
        {
            repositorioAluno.Remove(aluno);
        }
    }
}



