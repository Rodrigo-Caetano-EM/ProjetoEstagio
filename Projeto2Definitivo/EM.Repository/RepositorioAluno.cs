using FirebirdSql.Data.FirebirdClient;
using ProjetoDeEstagio2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace EM.Repository
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>
    {
        public RepositorioAluno() { }

        private FbConnection CrieConexao()
        {
            string conn = @"DataSource=localhost;Port=3054;Database=C:\Users\EscolarManager\Documents\DataBase ProjetoForms\DBPROJETO2.FB4;username=SYSDBA;password=masterkey";
            return new FbConnection(conn);
        }


        public override void Add(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                string sqlInsert = @"INSERT into TBALUNO (MATRICULA, NOME, CPF, NASCIMENTO, SEXO) VALUES(@matricula, @nome, @cpf, @nascimento, @sexo);";
                var teste = conexaoFireBird;
                var cmd = teste.CreateCommand();
                cmd.CommandText = sqlInsert;
                cmd.Parameters.Add("@matricula", SqlDbType.Int);
                cmd.Parameters["@matricula"].Value = aluno.Matricula;
                cmd.Parameters.Add("@nome", SqlDbType.VarChar);
                cmd.Parameters["@nome"].Value = aluno.Nome;
                cmd.Parameters.Add("@cpf", SqlDbType.VarChar);
                cmd.Parameters["@cpf"].Value = aluno.CPF;
                cmd.Parameters.Add("@nascimento", SqlDbType.DateTime);
                cmd.Parameters["@nascimento"].Value = aluno.Nascimento;
                cmd.Parameters.Add("@sexo", SqlDbType.Int);
                cmd.Parameters["@sexo"].Value = aluno.Sexo;
                conexaoFireBird.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public override void Remove(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                conexaoFireBird.Open();
                string mSQL = "DELETE from TBALUNO Where matricula = " + aluno.Matricula + ";";
                FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                cmd.ExecuteNonQuery();
            }
        }

        public override void Update(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                string sqlUpdate = $@"UPDATE TBALUNO SET MATRICULA = @matricula, NOME = @nome, CPF = @cpf, NASCIMENTO = @nascimento, SEXO = @sexo WHERE MATRICULA = {aluno.Matricula};";
                var teste = conexaoFireBird;
                var cmd = teste.CreateCommand();
                cmd.CommandText = sqlUpdate;
                cmd.Parameters.Add("@matricula", SqlDbType.Int);
                cmd.Parameters["@matricula"].Value = aluno.Matricula;
                cmd.Parameters.Add("@nome", SqlDbType.VarChar);
                cmd.Parameters["@nome"].Value = aluno.Nome;
                cmd.Parameters.Add("@cpf", SqlDbType.VarChar);
                cmd.Parameters["@cpf"].Value = aluno.CPF;
                cmd.Parameters.Add("@nascimento", SqlDbType.DateTime);
                cmd.Parameters["@nascimento"].Value = aluno.Nascimento;
                cmd.Parameters.Add("@sexo", SqlDbType.Int);
                cmd.Parameters["@sexo"].Value = aluno.Sexo;
                conexaoFireBird.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            return GetAll().Where(predicate.Compile());
        }

        public IEnumerable<Aluno> GetAll()
        {
            using (FbConnection conexaoFireBird = CrieConexao())
            {
                conexaoFireBird.Open();
                string consulta = "SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM TBALUNO ORDER BY MATRICULA";
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
        }

        public Aluno GetByMatricula(int matricula)
        {
            return Get(m => m.Matricula == matricula).First();
        }

        public IEnumerable<Aluno> GetByNome(string nome)
        {
            return Get(n => n.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
        }
    }
}