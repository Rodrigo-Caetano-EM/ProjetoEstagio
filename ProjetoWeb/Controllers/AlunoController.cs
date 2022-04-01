using EMRepository;
using Microsoft.AspNetCore.Mvc;
using ProjetoDeEstagio2;
using ProjetoWeb.Models;
using System.Text.RegularExpressions;

namespace ProjetoWeb.Controllers
{
    public class AlunoController : Controller
    {
        RepositorioAluno repositorioAluno = new();
        Validacoes validacoes = new();

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
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarAluno(Aluno aluno)
        {
            ValidarDadosInseridos(aluno);
            if (!string.IsNullOrEmpty(aluno.Matricula.ToString()))
            {
                if (validacoes.JaTemEssaMatricula(aluno.Matricula.ToString()))
                {
                    ModelState.AddModelError("Matricula", "Matricula já inserida");
                }
            }
            if (ModelState.IsValid)
            {
                repositorioAluno.Add(aluno);
                return RedirectToAction("SelecionarAluno");
            }
            else
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
        [ValidateAntiForgeryToken]
        public ActionResult AtualizarAluno([Bind(include: "Matricula, Nome, Sexo, Nascimento, CPF")] Aluno aluno)
        {
            ValidarDadosInseridos(aluno);
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

        public ActionResult Delete(Aluno aluno)
        {
            Aluno alunoASerExcluido = repositorioAluno.GetByMatricula(aluno.Matricula);

            if (alunoASerExcluido == null)
            {
                ViewBag.Message = "Aluno não foi encontrado";
                return RedirectToAction("SelecionarAluno");
            }
            return View(alunoASerExcluido);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Aluno aluno)
        {
            Aluno alunoASerExcluido = repositorioAluno.GetByMatricula(aluno.Matricula);
            repositorioAluno.Remove(alunoASerExcluido);

            return RedirectToAction("SelecionarAluno");
        }
        private void ValidarDadosInseridos(Aluno aluno)
        {
            if (!string.IsNullOrEmpty(aluno.CPF))
            {
                if (!validacoes.EhCPFValido(aluno.CPF))
                {
                    ModelState.AddModelError("CPF", "CPF inválido");
                }
            }
            if (!string.IsNullOrEmpty(aluno.CPF) && validacoes.JaTemEsseCPF(aluno.Matricula.ToString(), aluno.CPF))
            {
                ModelState.AddModelError("CPF", "CPF já inserido");
            }
            
        }
        public ActionResult Index(string id, Aluno aluno)
        {
            var alunosSelecionados = from a in repositorioAluno.GetAll() select a;

            if (!string.IsNullOrEmpty(id))
            {
                alunosSelecionados = alunosSelecionados.Where(s => s.Nome.Contains(id));
            }

            if (!string.IsNullOrEmpty(id))
            {
                alunosSelecionados = alunosSelecionados.Where(s => s.Nome.Contains(id, StringComparison.InvariantCultureIgnoreCase));
                return View(alunosSelecionados);
            }

            if (aluno.Matricula != 0)
            {
                alunosSelecionados = alunosSelecionados.Where(s => s.Matricula == aluno.Matricula);
                return View(alunosSelecionados);
            }

            return View(alunosSelecionados);
        }
    }
}