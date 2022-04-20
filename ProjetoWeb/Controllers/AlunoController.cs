using EMRepository;
using Microsoft.AspNetCore.Mvc;
using ProjetoDeEstagio2;
using ProjetoWeb.Models;
using System.Text.RegularExpressions;

namespace ProjetoWeb.Controllers
{
    public class AlunoController : Controller
    {
        readonly RepositorioAluno repositorioAluno = new();
        readonly Validacoes validacoes = new();

        public ActionResult PesquisarAluno()
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
            ImprimaMensagemCPF(aluno);
            ImprimaMensagemMatricula(aluno);
            ImprimMensagemNome(aluno);
            ImprimaMensagemNascimento(aluno.Nascimento);
            if (validacoes.EhValido(aluno.Nome, aluno.Nascimento, aluno.CPF))
            {
                if(!validacoes.JaTemEssaMatricula(aluno.Matricula))
                {
                    if (!string.IsNullOrEmpty(aluno.CPF))
                    {
                        aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                        repositorioAluno.Add(aluno);
                        return RedirectToAction("PesquisarAluno");
                    }
                    if (string.IsNullOrEmpty(aluno.CPF))
                    {
                        repositorioAluno.Add(aluno);
                        return RedirectToAction("PesquisarAluno");
                    }
                }               
            }
            return View();
        }

        public ActionResult Edit(int matricula)
        {
            Aluno alunoASerEditado = repositorioAluno.GetByMatricula(matricula);

            return View(alunoASerEditado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(include: "Matricula, Nome, Sexo, Nascimento, CPF")] Aluno aluno)
        {
            ImprimaMensagemCPF(aluno);
            ImprimMensagemNome(aluno);
            ImprimaMensagemNascimento(aluno.Nascimento);
            if (ModelState.IsValid && !string.IsNullOrEmpty(aluno.CPF) && validacoes.EhValido(aluno.Nome, aluno.Nascimento, aluno.CPF))
            {
                aluno.CPF = aluno.CPF.Replace(".", "").Replace("-", "");
                repositorioAluno.Update(aluno);
                return RedirectToAction("PesquisarAluno");
            }
            if (ModelState.IsValid && string.IsNullOrEmpty(aluno.CPF) && validacoes.EhValido(aluno.Nome, aluno.Nascimento, aluno.CPF))
            {
                repositorioAluno.Update(aluno);
                return RedirectToAction("PesquisarAluno");
            }
            else
            {
                return View(aluno);
            }
        }

        public ActionResult Delete(Aluno aluno)
        {
            Aluno alunoASerExcluido = repositorioAluno.GetByMatricula(aluno.Matricula);

            return View(alunoASerExcluido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Aluno aluno)
        {
            Aluno alunoASerExcluido = repositorioAluno.GetByMatricula(aluno.Matricula);
            repositorioAluno.Remove(alunoASerExcluido);

            return RedirectToAction("PesquisarAluno");
        }
        [HttpPost]
        public ActionResult PesquisarAluno(string idInserido)
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
                        IEnumerable<Aluno> alunos = repositorioAluno.GetAll();

                        return View(alunos);
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
                        IEnumerable<Aluno> alunos = repositorioAluno.GetAll();

                        return View(alunos);
                    }
                }
            }
            return RedirectToAction("PesquisarAluno", "Aluno");
        }

        private void ImprimaMensagemNascimento(DateTime nascimento)
        {
            DateTime dataNascimentoMinima = new(1900, 01, 01, 00, 00, 00);
            DateTime dataAtual = DateTime.Now;
            var CompararDatas = DateTime.Compare(dataNascimentoMinima, nascimento);
            int ComparaAno = dataAtual.Year - nascimento.Year;
            string tamanhoDoAno = nascimento.Year.ToString();
            if(tamanhoDoAno.Length != 4)
            {
                ModelState.AddModelError(string.Empty, "Ano inválido");
            }
            if(nascimento.Year > dataAtual.Year)
            {
                if (CompararDatas < 0 || dataAtual.Year < nascimento.Year)
                {
                    ModelState.AddModelError(string.Empty, "Data inválida");
                }
            }
            if(ComparaAno <= 1)
            {
                TimeSpan testeTempoDecorrido = DateTime.Now - nascimento;
                float qntdMeses = testeTempoDecorrido.Days / 30;
                if (qntdMeses < 6)
                {
                    ModelState.AddModelError(string.Empty, "Idade insuficiente");
                }
            }
        }
        private void ImprimaMensagemCPF(Aluno aluno)
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
        private void ImprimMensagemNome(Aluno aluno)
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
        private void ImprimaMensagemMatricula(Aluno aluno)
        {
            int numeroDaMatricula = Convert.ToInt32(aluno.Matricula);

            if (numeroDaMatricula <= 0)
            {
                ModelState.AddModelError(string.Empty, "Matricula inválida");
            }
            if (!string.IsNullOrEmpty(aluno.Matricula.ToString()))
            {
                if (validacoes.JaTemEssaMatricula(aluno.Matricula))
                {
                    ModelState.AddModelError(string.Empty, "Matricula já inserida");
                }
            }
        }
        
    }   
}