using Syngenta.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syngenta.Core.Contracts.Data
{
    public interface IFailedLoginAttemptRepository : IRepository<FailedLoginAttempt>
    {
        IEnumerable<FailedLoginAttempt> GetByMail(string email);
    }
}
