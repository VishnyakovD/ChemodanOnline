using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace Shop.db.Repository
{
    public interface IRepository
    {
        object OneAsObject(object id);
    }


    public interface IRepository<T> 
    {
        List<T> All();
        T One(object id);
        T TryOne(object id);
        T Add(T obj);
        void AddMany(List<T> obj);
        void Update(T obj);
        void Remove(T obj);
        void Refresh(T obj);
        void Flush();
        void AddOrUpdate(T obj);
        Type GetRealClass(object obj);
        void AddOrUpdateMany(List<T> obj);
    }

    public class Repository<T> : IRepository, IRepository<T> where T : class
    {
        protected ISession session;

        public Repository(ISession session)
        {
            this.session = session;
        }

        public Type GetRealClass(object obj)
        {
            return NHibernateUtil.GetClass(obj);
        }

        public List<T> All()
        {
            List<T> retval = new List<T>();
            retval.AddRange(session.CreateCriteria(typeof(T)).List<T>());
            return retval;
        }

        public T Add(T obj)
        {
            session.Save(obj);
            return obj;
        }

        public void AddOrUpdate(T obj)
        {
            session.SaveOrUpdate(obj);
        }


        public void AddMany(List<T> obj)
        {
            for(int i=0;i<obj.Count;i++)
            {
                session.Save(obj[i]);
            }            
        }

        public void AddOrUpdateMany(List<T> obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                session.SaveOrUpdate(obj[i]);
            }
        }

        public void Flush()
        {
            session.Flush();
        }


        public T One(object id)
        {
            T retval;
            if ((long) id < 1)
            {
                return null;
            }
            retval = session.Get<T>(id);
                if (retval == null)
                    throw new Exception(String.Format("{0} id={1} не найден", typeof (T), id));
           
            return retval;
        }

        public void Refresh(T obj)
        {
            session.Refresh(obj);
        }


        public T TryOne(object id)
        {
            T retval = default(T);
            try
            {
                if ((long)id>0)
                {
                    retval = One(id); 
                }
            
            }
            catch
            {
            }
            return retval;
        }


        public void Update(T obj)
        {
            session.Update(obj);
            session.Flush();
        }

        public void Remove(T obj)
        {
            session.Delete(obj);
            session.Flush();
        }



        public List<T> Many(long[] ids)
        {
            return
                new List<T>(
                    session
                    .CreateCriteria(typeof(T))
                    .Add(Expression.In("id", ids))
                    .List<T>()
                );
        }


        public object OneAsObject(object id)
        {
            return One(id);
        }
    }
}
