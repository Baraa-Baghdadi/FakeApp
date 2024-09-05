using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace DawaaNeo.Patients
{
    public class CurrentPatient : ICurrentPatient , ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        public CurrentPatient(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public string getUserNameFromToken()
        {
            var userName = _currentUser.UserName;
            return userName is not null ? userName : "Invalid Token";
        }
    }
}
