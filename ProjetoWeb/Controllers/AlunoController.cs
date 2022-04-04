using EMRepository;
using Microsoft.AspNetCore.Mvc;
using ProjetoDeEstagio2;
using ProjetoWeb.Models;

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
            if (ModelState.IsValid && !string.IsNullOrEmpty(aluno.CPF))
            {
                aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                repositorioAluno.Add(aluno);
                return RedirectToAction("SelecionarAluno");
            }
            if (ModelState.IsValid && string.IsNullOrEmpty(aluno.CPF))
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
            if (ModelState.IsValid && !string.IsNullOrEmpty(aluno.CPF))
            {
                aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                repositorioAluno.Update(aluno);
                return RedirectToAction("SelecionarAluno");
            }
            if (ModelState.IsValid && string.IsNullOrEmpty(aluno.CPF))
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
        [HttpPost]
        public ActionResult SelecionarAluno(string idInserido)
        {
            if (!string.IsNullOrEmpty(idInserido))
            {
                if (int.TryParse(idInserido, out int Pesquisa))
                {
                    try
                    {
                        List<Aluno> alunos = new()
                        {
                            repositorioAluno.GetByMatricula(Pesquisa)
                        };
                        return View(alunos);
                    }
                    catch
                    {
                        ModelState.AddModelError("idInserido", "Aluno não encontado");
                    }
                }
                if (repositorioAluno.GetByNome(idInserido).Any())
                {
                    IEnumerable<Aluno> alunos = repositorioAluno.GetByNome(idInserido);

                    return View(alunos);
                }
                if (!repositorioAluno.GetByNome(idInserido).Any())
                {
                    return RedirectToAction("SelecionarAluno");
                }
            }
            else
            {
                return RedirectToAction("SelecionarAluno");
            }
            return View();
        }
    }
}