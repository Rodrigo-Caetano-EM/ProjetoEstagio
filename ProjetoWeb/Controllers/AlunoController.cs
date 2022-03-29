using EMRepository;
using Microsoft.AspNetCore.Mvc;
using ProjetoDeEstagio2;

namespace ProjetoWeb.Controllers
{
    public class AlunoController : Controller
    {
        RepositorioAluno repositorioAluno = new();
        public ActionResult SelecionarAluno()
        {
            ModelState.Clear();

            return View(repositorioAluno.GetAll());
        }

        public ActionResult AdicionarAluno()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarAluno(Aluno aluno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioAluno.Add(aluno);
                    return RedirectToAction("SelecionarAluno");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AtualizarAluno(int matricula)
        {
            Aluno teste = repositorioAluno.GetByMatricula(matricula);

            return View(teste);
        }

        [HttpPost]

        public ActionResult AtualizarAluno([Bind(include: "Matricula, Nome, Sexo, Nascimento, CPF")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                repositorioAluno.Update(aluno);
                return RedirectToAction("SelecionarAluno");
            }
            else
            {
                return View(aluno);
            }
        }

        public ActionResult ExcluirAluno(Aluno aluno)
        {
            repositorioAluno.Remove(aluno);

            return RedirectToAction("SelecionarAluno");
        }
    }
}