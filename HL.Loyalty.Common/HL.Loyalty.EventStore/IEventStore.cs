using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HL.Loyalty.EventStore
{
    public interface IEventStore
    {
        Task<bool> SaveAsync<T>(T obj);

        Task<bool> SaveAsync<T>(T obj, string partitionKey);

        List<T> Read<T>(string id);

        List<T> Read<T>(Expression<Func<T, bool>> expression);
    }
}
