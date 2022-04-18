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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            ValidarCPF(aluno);
            ValidarMatricula(aluno);
            ValidarNome(aluno);
            ValidarDataNascimento(aluno.Nascimento);
            if (validacoes.EhValido(aluno.Nome, aluno.Nascimento))
            {
                if (!string.IsNullOrEmpty(aluno.CPF))
                {
                    aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                    repositorioAluno.Add(aluno);
                    return RedirectToAction("SelecionarAluno");
                }
                if (string.IsNullOrEmpty(aluno.CPF))
                {
                    repositorioAluno.Add(aluno);
                    return RedirectToAction("SelecionarAluno");
                }
            }
            return View();
        }

        public ActionResult Edit(int matricula)
        {
            Aluno teste = repositorioAluno.GetByMatricula(matricula);

            return View(teste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(include: "Matricula, Nome, Sexo, Nascimento, CPF")] Aluno aluno)
        {
            ValidarCPF(aluno);
            ValidarNome(aluno);
            if (ModelState.IsValid && !string.IsNullOrEmpty(aluno.CPF) && validacoes.EhValido(aluno.Nome, aluno.Nascimento))
            {
                aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                repositorioAluno.Update(aluno);
                return RedirectToAction("SelecionarAluno");
            }
            if (ModelState.IsValid && string.IsNullOrEmpty(aluno.CPF) && validacoes.EhValido(aluno.Nome, aluno.Nascimento))
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

        private void ValidarDataNascimento(DateTime nascimento)
        {
            DateTime dataNascimentoMinima = new(1900, 01, 01, 00, 00, 00);
            DateTime dataDeTesteAluno = nascimento;
            DateTime dataAtual = DateTime.Now;
            var CompararDatas = DateTime.Compare(dataNascimentoMinima, dataDeTesteAluno);
            int idade = dataAtual.Year - dataDeTesteAluno.Year;
            if(dataDeTesteAluno.Year >= dataAtual.Year)
            {
                if (CompararDatas < 0 || dataAtual.Year < dataDeTesteAluno.Year)
                {
                    ModelState.AddModelError(string.Empty, "Data inválida");
                }
            }
            int mesNascimento = Convert.ToInt32(dataDeTesteAluno.Month) + 12;
            int mesAtual = Convert.ToInt32(dataAtual.Month) + 12;
            if (Math.Abs(mesAtual - mesNascimento) >= 7 && idade <= 1)
            {
                ModelState.AddModelError(string.Empty, "Idade insuficiente");
            }
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
        private void ValidarNome(Aluno aluno)
        {
            if (!string.IsNullOrEmpty(aluno.Nome))
            {
                if (!Regex.IsMatch(aluno.Nome, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ModelState.AddModelError(string.Empty, "Nome inválido");
                }

                if (aluno.Nome.Length < 1)
                {
                    ModelState.AddModelError(string.Empty, "O nome precisa ser preenchido");
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
            var falha = false;
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
                        falha = true;
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
                        falha = true;
                    }
                }
                else
                {
                        falha = true;
                }
            }
            if (falha)
            {
                ModelState.AddModelError(string.Empty, "Aluno não encontrado");
            }
            return RedirectToAction("SelecionarAluno", "Aluno");
        }
    }   
}