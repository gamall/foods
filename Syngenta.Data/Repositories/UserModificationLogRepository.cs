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
    public class UserModificationLogRepository : EFRepository<UserModificationLog>, IUserModificationLogRepository
    {
        public UserModificationLogRepository(DbContext context) : base(context)
        {
        }
    }
}
