using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Patients
{
    public interface IPatientAddressRepository : IRepository<PatientAddress,Guid>
    {
        Task<(List<PatientAddress>,int)> GetUserAddresses(Guid patientId,string?name,string?appartmentNumber,string?buildingName,
            string longitude,string?latitude,string?landmark,string?address, PatientAddressType? type,int skipCount,int maxResultCount,string?sorting);
    }
}
