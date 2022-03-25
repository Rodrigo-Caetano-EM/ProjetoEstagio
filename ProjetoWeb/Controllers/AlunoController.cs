using Microsoft.AspNetCore.Mvc;
using ProjetoWeb.Models;
using ProjetoWeb.Repository;
using ProjetoDeEstagio2;

namespace ProjetoWeb.Controllers
{
    public class AlunoController : Controller
    {
        public ActionResult SelecionarAluno()
        {
            var alunoRepository = new AlunoRepository();
            ModelState.Clear();

            return View(alunoRepository.SelecionarAlunos());
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
                    var alunoRespository = new AlunoRepository();

                    alunoRespository.AdicionarAluno(aluno);
                }
                
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AtualizarAluno(Aluno aluno)
        {
            var alunoRepository = new AlunoRepository();

            return View(alunoRepository.SelecionarAlunos().Where(a => a.Matricula == aluno.Matricula));

        }

        public ActionResult ExcluirAluno(Aluno aluno)
        {
            var alunoRepository = new AlunoRepository();

            alunoRepository.ExcluirAluno(aluno);

            return RedirectToAction("SelecionarAlunos");
        }
    }
}