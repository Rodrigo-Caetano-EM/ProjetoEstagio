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
            ValidarCPF(aluno);
            ValidarMatricula(aluno);
            if (validacoes.EhValido(aluno.Nome, aluno.Nascimento))
            {
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
            }
            return View();
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
            ValidarCPF(aluno);
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

        public bool ValidarSexo(Sexo sexo)
        {
            if (sexo.CategoriaId == 0)
            {
                return false;
            }
            return true;
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
        [HttpPost]
        private void ValidarCPF(Aluno aluno)
        {
            if (!string.IsNullOrEmpty(aluno.CPF))
            {
                if (!Validacoes.EhCPFValido(aluno.CPF))
                {
                    ModelState.AddModelError(string.Empty, "CPF inválido");
                }
                if (Validacoes.JaTemEsseCPF(aluno.Matricula.ToString(), aluno.CPF))
                {
                    ModelState.AddModelError(string.Empty, "CPF já inserido");
                }
            }
        }
        [HttpPost]
        private void ValidarMatricula(Aluno aluno)
        {
            int numeroDaMatricula = Convert.ToInt32(aluno.Matricula);

            if (numeroDaMatricula <= 0)
            {
                ModelState.AddModelError(string.Empty, "Matricula inválida");
            }
            if (!string.IsNullOrEmpty(aluno.Matricula.ToString()))
            {
                if (Validacoes.JaTemEssaMatricula(aluno.Matricula.ToString()))
                {
                    ModelState.AddModelError(string.Empty, "Matricula já inserida");
                }
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
                        ModelState.AddModelError(string.Empty, "Aluno não encontado");
                    }
                }
                else if (repositorioAluno.GetByNome(idInserido).Any())
                {
                    try
                    {
                        IEnumerable<Aluno> alunos = repositorioAluno.GetByNome(idInserido);

                        return View(alunos);
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Aluno não encontrado");

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Aluno não encontrado");
                }
            }
            return RedirectToAction("SelecionarAluno", "Aluno");
        }
    }
}