using Task.models;
using System.Collections.Generic;

namespace Task.interfaces
{
    public interface IListTaskService
    {
        List<task> GetAll(long userId);
        task Get(long userId, int id);
        void Add(long userId, task task);
        void Delete(long userId, int id);
        void Update(long userId, task task);
        int Count(long userId);
    }
}