﻿using FirebirdSql.Data.FirebirdClient;
using ProjetoDeEstagio2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EM.Repository
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>
    { 
        private RepositorioAluno() { }

        private FbConnection CrieConexao()
        {
            string conn = @"DataSource=localhost;Port=3054;Database=C:\Users\EscolarManager\Desktop\Nova pasta\DBPROJETO2.FB4;username=SYSDBA;password=masterkey";
            return new FbConnection(conn);
        }


        public override void Add(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                conexaoFireBird.Open();
                string mSQL = "INSERT into TBALUNO Values(" + aluno.Matricula + ",'" + aluno.Nome + "','" + aluno.CPF + "','" +
                aluno.Nascimento.ToString("dd.MM.yyyy") + "','" + (int)aluno.Sexo + "')";

                FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                cmd.ExecuteNonQuery();
            }
        }

        public override void Remove(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "DELETE from TBALUNO Where matricula = " + aluno.Matricula + ";";
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        public override void Update(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = GetInstancia().GetConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "Update TBALUNO SET MATRICULA = '" + Convert.ToInt32(aluno.Matricula) +
                                                 "', NOME = '" + aluno.Nome +
                                                 "', CPF = '" + aluno.CPF +
                                                 "', NASCIMENTO= '" + aluno.Nascimento.ToString("yyyy-MM-dd") +
                                                 "', SEXO= '" + Convert.ToInt32(aluno.Sexo) + "'" +
                                                 " Where MATRICULA= " + aluno.Matricula + ";";
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            return GetAll().Where(predicate.Compile());
        }

        public IEnumerable<Aluno> GetAll()
        {
            using (FbConnection conexaoFireBird = GetConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string consulta = "SELECT * FROM TBALUNO";
                    FbCommand cmd = new FbCommand(consulta, conexaoFireBird);
                    FbDataReader dr = cmd.ExecuteReader(); // estudar
                    List<Aluno> alunos = new List<Aluno>();
                    while (dr.Read())
                    {
                        alunos.Add(new Aluno()
                        {
                            Matricula = Convert.ToInt32(dr["MATRICULA"]),
                            Nome = dr["NOME"].ToString(),
                            CPF = dr["CPF"].ToString(),
                            Nascimento = Convert.ToDateTime(dr["NASCIMENTO"]),
                            Sexo = (EnumeradorSexo)Convert.ToInt32(dr["SEXO"].ToString())
                        });
                    }
                    dr.Close();
                    return alunos;
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        public Aluno GetByMatricula(int matricula)
        {
            try
            {
                return Get(m => m.Matricula == matricula).First();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Aluno inexistente");
                return null;
            }
        }

        public IEnumerable<Aluno> GetByNome(string nome)
        {
            try
            {
                return Get(n => n.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Aluno inexistente");
                return null;
            }
        }
    }
}