using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CalculadoraAnestesica.DataAccess.Base.Interface
{
    public interface IDataAccessBase<TI, TM>
    {
        void Delete(TI entity);
        void Insert(TI entity);
        IList<TM> SelectAllItems();
        IList<TM> SelectByCriteria(Expression<Func<TM, bool>> predicate);
        void Update(TI entity);
    }
}

