using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reposatiory
{
    public class MainManager<T> where T : class
    {
        private readonly LoftyContext MydB;
        private readonly DbSet<T> set;
        public MainManager(LoftyContext _mydB)
        {
            MydB = _mydB;
            set = MydB.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return set.AsQueryable();
        }
        public async Task<T> FindByIdAsync(int id)
        {
            return await set.FindAsync(id);
        }
        //public async Task<T> FindByStringIdAsync(string id)
        //{
        //    return await set.FindAsync(id);
        //}
        public async Task AddAsync(T entity)
        {
            await set.AddAsync(entity);
        }
        public void Update(T entity)
        {
            set.Update(entity);
        }
        public void PartialUpdate(T entity)
        {
            set.Attach(entity);
        }
        //public void PartialUpdate2(T entity, params Expression<Func<T, object>>[] propertiesToUpdate)
        //{
        //    set.Attach(entity);
        //    foreach (var property in propertiesToUpdate)
        //    {
        //        MydB.Entry(entity).Property(property).IsModified = true;
        //    }
        //}
        public void Remove(T entity)
        {
            set.Remove(entity);
        }
    }
}
