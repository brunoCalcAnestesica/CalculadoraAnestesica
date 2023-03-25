using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CalculadoraAnestesica.DataAccess.Base.Interface;
using CalculadoraAnestesica.DbContext.Tables;
using SQLite;

namespace CalculadoraAnestesica.DataAccess.Base
{
	public class DataAccessBase<TI, TM> : IDataAccessBase<TI, TM> where TM : new()
    {
        protected SQLiteConnection Connection {
            get { return DatabaseHandler.Instance.Connection; }
        }

        public virtual void Delete(TI entity)
        {
            Connection.Delete(entity);
        }

        public virtual void Insert(TI entity)
        {
            Connection.Insert(entity);
        }

        public virtual IList<TM> SelectAllItems()
        {
            return Connection
                .Table<TM>()
                .ToList();
        }

        public virtual IList<TM> SelectByCriteria(Expression<Func<TM, bool>> predicate)
        {
            return Connection
                .Table<TM>()
                .Where(predicate)
                .ToList();
        }

        public virtual void Update(TI entity)
        {
            Connection.Update(entity);
        }
    }
}

