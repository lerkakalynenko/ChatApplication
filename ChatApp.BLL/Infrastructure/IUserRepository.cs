using System.Collections.Generic;
using ChatApp.DAL.Entities;

namespace ChatApp.BLL.Infrastructure
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetUsers(string id);
    }
}
