using mTask.models;
using System.Collections.Generic;

namespace mTask.interfaces{

    public interface IUserService{
    
        List<User> GetAll();
        User Get(long userId);
        void Post(User u);
        void Delete(int id);
    }

}