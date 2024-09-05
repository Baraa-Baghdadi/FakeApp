using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace DawaaNeo.Patients
{
    public class PatientManager : DomainService
    {
        protected IPatientRepository _patientRepository;
        private readonly IPatientAddressRepository _patientAddressRepository;
        public PatientManager(IPatientRepository patientRepository, IPatientAddressRepository patientAddressRepository)
        {
            _patientAddressRepository = patientAddressRepository;
            _patientRepository = patientRepository;
        }

        public virtual async Task<Patient> CreateAsync(string mobileNumber,string countryCode,string patientId)
        {
            var patient = new Patient(GuidGenerator.Create(),mobileNumber,countryCode,patientId);
            return await _patientRepository.InsertAsync(patient);
        }

        public virtual async Task<Patient> UpdateAsync(Guid id,string mobileNumber,string countryCode,string concurrencyStamp = null)
        {
            var patient = await _patientRepository.GetAsync(id);
            patient.MobileNumber = mobileNumber;
            patient.CountryCode = countryCode;
            patient.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _patientRepository.UpdateAsync(patient);
        }

        public virtual async Task<PatientAddress> GetAddressAsync(Guid id)
        {
            var address = await _patientAddressRepository.GetAsync(id);
            if (address == null)
            {
                throw new UserFriendlyException("Not Found");
            }
            return address;
        }

        public virtual async Task<PatientAddress> CreateAddressAsync(Guid patientId,string name,string buildingName,
            string appartmentNumber,string landMark,string longitude,string latitude,string address,bool isDefault)
        {
            var patientAddress = new PatientAddress(GuidGenerator.Create(), patientId, name, buildingName, 
                appartmentNumber, landMark, longitude, latitude, address, isDefault);

            return await _patientAddressRepository.InsertAsync(patientAddress,true);
        }

        public virtual async Task<PatientAddress> UpdateAddressAsync(Guid id, string name, string buildingName,
            string appartmentNumber, string landMark, string longitude, string latitude, string address, 
            bool isDefault,PatientAddressType? addressType)
        {
            var patientAddress = await _patientAddressRepository.GetAsync(id);
            if (patientAddress == null) throw new UserFriendlyException("Not Found");
            patientAddress.Name = name;
            patientAddress.BuildingName = buildingName; 
            patientAddress.AppartmentNumber = appartmentNumber;
            patientAddress.LandMark = landMark;
            patientAddress.Longitude = longitude;
            patientAddress.Latitude = latitude;
            patientAddress.Address = address;
            patientAddress.IsDefault = isDefault;

            if (isDefault)
            {
                var defaultAddress = await _patientAddressRepository.FirstOrDefaultAsync(x => x.IsDefault && x.PatientId 
                == patientAddress.PatientId && x.Id != patientAddress.Id);
                if (defaultAddress is not null)
                {
                    defaultAddress.IsDefault = false;
                    await _patientAddressRepository.UpdateAsync(defaultAddress, true);
                }
            }
            return await _patientAddressRepository.UpdateAsync(patientAddress,true);
        }

        public virtual async Task<bool> DeleteAddressAsync(Guid id)
        {
            var address = await _patientAddressRepository.GetAsync(id);
            if (address is null) throw new UserFriendlyException("Not Found");
            await _patientAddressRepository.DeleteAsync(address,true);
            return true;
        }

        public virtual async Task<bool> SetAddressAsDefault(Guid id)
        {
            var address = await _patientAddressRepository.GetAsync(id);
            if (address is null) throw new UserFriendlyException("Not Found");
            var defaultAddress = await _patientAddressRepository.FirstOrDefaultAsync(
            a => a.IsDefault && a.PatientId == address.PatientId    
            );
            if (defaultAddress is not null)
            {
                if(address.Id == defaultAddress.Id) return true;
                defaultAddress.IsDefault = false;
                await _patientAddressRepository.UpdateAsync(defaultAddress, true);
            }
            address.IsDefault = true;
            await _patientAddressRepository.UpdateAsync(address, true);
            return true;
        }
    }
}
