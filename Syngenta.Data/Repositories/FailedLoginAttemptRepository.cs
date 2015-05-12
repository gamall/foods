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
    public class FailedLoginAttemptRepository : EFRepository<FailedLoginAttempt>, IFailedLoginAttemptRepository
    {
        public FailedLoginAttemptRepository(DbContext context) : base(context)
        {            
        }

        public IEnumerable<FailedLoginAttempt> GetByMail(string email)
        {
            return DbSet.Where(o => o.EmailAddress.Contains(email));
        }
    }
}
