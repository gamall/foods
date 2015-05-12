using Syngenta.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syngenta.Core.Contracts.Data
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
        User GetBySurname(string name);

        IEnumerable<User> GetByMail(string email);
        IEnumerable<User> GetByName(string name);

        IQueryable<User> GetByIsNotAdmin();
        IQueryable<User> AdminSearch(User feedsUser);

        User IsDisabled(int userId);

    }
}
