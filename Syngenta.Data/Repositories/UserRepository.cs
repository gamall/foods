using Syngenta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syngenta.Core.Contracts.Data;
using System.Data.Entity;

namespace Syngenta.Data.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context)
            : base(context)
        {
        }
        
        public User GetByEmail(string email)
        {
            return DbSet.FirstOrDefault(o => o.Email == email);
        }

        public User GetBySurname(string name)
        {
            return DbSet.FirstOrDefault(o => o.Surname == name);
        }

        public IEnumerable<User> GetByMail(string email)
        {
            return DbSet.Where(o => o.Email.Contains(email));
        }

        public IEnumerable<User> GetByName(string name)
        {
            return DbSet.Where(o => o.Surname.Contains(name)).Union(DbSet.Where(o => o.Forename.Contains(name)));
        }

        public IQueryable<User> GetByIsNotAdmin()
        {
            return DbSet.Where(o => !o.IsAdmin);
        }

        public IQueryable<User> AdminSearch(User feedsUser)
        {
            if (!String.IsNullOrEmpty(feedsUser.Email) || !String.IsNullOrEmpty(feedsUser.Surname) || !String.IsNullOrEmpty(feedsUser.Forename))
            {
                return DbSet.Where(o => o.Forename.ToLower().Contains(feedsUser.Forename))
                    .Union(DbSet.Where(o => o.Surname.ToLower().Contains(feedsUser.Surname)))
                    .Where(o => o.Email.ToLower().Contains(feedsUser.Email))
                    .Where(o => !o.IsAdmin);
            }
            else
            {
                return DbSet.Where(o => !o.IsAdmin);
            }
        }

        public User IsDisabled(int userId)
        {
            return DbSet.FirstOrDefault(o => o.Id == userId);
        }

    }
}
