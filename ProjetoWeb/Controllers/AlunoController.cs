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
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditAluno(Aluno aluno)
        {
            Aluno teste = repositorioAluno.GetByMatricula(aluno.Matricula);

            return View(teste);
        }

        public ActionResult ExcluirAluno(Aluno aluno)
        {
            repositorioAluno.Remove(aluno);

            return RedirectToAction("SelecionarAluno");
        }
    }
}