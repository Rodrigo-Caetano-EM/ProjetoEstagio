using ProjetoDeEstagio2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EMRepository
{
    public abstract class RepositorioAbstrato<T> where T : IEntidade
    {
        public abstract void Add(T pessoa);
        public virtual void Remove(T pessoa) { }
        public virtual void Update(T pessoa) { }
        public virtual IEnumerable<T> GetAll(T pessoa) { return null; }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate) { return null; } // estudar
    }
}