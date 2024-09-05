using DawaaNeo.ApiResponse;
using DawaaNeo.Providers;
using DawaaNeo.Providers.Mobile;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Patients.Mobile
{
    [Authorize]
    public class PatientMobileAppService : ApplicationService , IPatientMobileAppService
    {
        protected IPatientRepository _patientRepository;
        protected IPatientProviderRepository _patientProviderRepository;
        protected ProviderManager _providerManager;
        protected IApiResponse _apiResponse;
        protected ICurrentPatient _currentPatient;
        protected PatientManager _patientManager;
        protected IPatientAddressRepository _patientAddressRepository;
        public PatientMobileAppService(
            IPatientRepository patientRepository,
            IPatientProviderRepository patientProviderRepository,
            ProviderManager providerManager,
            IApiResponse apiResponse,
            ICurrentPatient currentPatient
            , PatientManager patientManager,
            IPatientAddressRepository patientAddressRepository
            )
        {
            _apiResponse = apiResponse;
            _patientRepository = patientRepository;
            _patientProviderRepository = patientProviderRepository;
            _providerManager = providerManager;
            _currentPatient = currentPatient;
            _patientManager = patientManager;
            _patientAddressRepository = patientAddressRepository;
        }

        #region patient address
        [Authorize]
        public async Task<Response<PatientAddressDto>> CreateAddressAsync(PatientAddressCreateDto input)
            {
                var userName = _currentPatient.getUserNameFromToken();
                var patient = await _patientRepository.GetPatientByMobileNumber(userName);
                if (patient == null) { throw new UserFriendlyException("Not Found"); }
                var createPatientAddress = await _patientManager.CreateAddressAsync(patient.Id,
                    input.Name, input.BuildingName, input.AppartmentNumber, input.LandMark, input.Longitude, input.Latitude,
                    input.Address, input.IsDefault);
                var response = ObjectMapper.Map<PatientAddress, PatientAddressDto>(createPatientAddress);
                return _apiResponse.Success<PatientAddressDto>(response);
            }

      [Authorize]
      public async Task<Response<bool>> DeleteAddress(Guid id)
          {
              try
              {
                  return _apiResponse.Success(await _patientManager.DeleteAddressAsync(id));
              }
              catch (UserFriendlyException e)
              {
                  throw new UserFriendlyException(e.Message);
              }
          }

      [Authorize]
      public async Task<Response<PatientAddressDto>> GetAddressAsync(Guid id)
          {
              try
              {
                  var address = await _patientManager.GetAddressAsync(id);
                  return _apiResponse.Success(ObjectMapper.Map<PatientAddress, PatientAddressDto>(address));
              }
              catch (UserFriendlyException e)
              {
                  throw new UserFriendlyException(e.Message);
              }
          }

      [Authorize]
      public async Task<Response<PagedResultDto<PatientAddressDto>>> GetAddressListAsync(GetPatientAddressInput input)
          {
              var userName = _currentPatient.getUserNameFromToken();
              var patient = await _patientRepository.GetPatientByMobileNumber(userName);
              if (patient == null) throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);
              var (addresses, totalcount) = await _patientAddressRepository.GetUserAddresses(patient.Id, input.Name,
                  input.AppartmentNumber, input.BuildingName, input.Longitude, input.Latitude, input.LandMark, input.Address,
                  input.Type,
                  input.SkipCount, input.MaxResultCount, input.Sorting);
              var response = ObjectMapper.Map<List<PatientAddress>, List<PatientAddressDto>>(addresses);
              var data = new PagedResultDto<PatientAddressDto>
              {
                  TotalCount = totalcount,
                  Items = response
              };
              return _apiResponse.Success<PagedResultDto<PatientAddressDto>>(data);
          }
      [Authorize]
      public async Task<Response<bool>> SetAddressAsDefault(Guid addressId)
          {

              try
              {
                  return _apiResponse.Success(await _patientManager.SetAddressAsDefault(addressId));
              }
              catch (UserFriendlyException e)
              {
                  throw new UserFriendlyException(e.Message);
              }
          }

      [Authorize]
      public async Task<Response<PatientAddressDto>> UpdateAddressAsync(PatientAddressUpdateDto input)
          {
              try
              {
                  var patientAddress = await _patientManager.UpdateAddressAsync(input.Id,
                      input.Name, input.BuildingName, input.AppartmentNumber, input.LandMark, input.Longitude, input.Latitude,
                      input.Address, input.IsDefault, input.Type);
                  return _apiResponse.Success(ObjectMapper.Map<PatientAddress, PatientAddressDto>(patientAddress));

              }
              catch (UserFriendlyException e)
              {
                  throw new UserFriendlyException(e.Message);
              }
              catch (Exception e)
              {
                  throw new UserFriendlyException(e.Message);
              }
          }
        #endregion

        #region Patiant Provider

        [Authorize]
        public async Task<Response<bool>> AddToMyPharmacy(PatientProviderCreateDto input)
        {
            var currentUserFromToken = _currentPatient.getUserNameFromToken();
            var patient = await _patientRepository.GetPatientByMobileNumber(currentUserFromToken);
            var listOfUserPharmacies = await _patientProviderRepository.GetListForMobileAsync(currentUserFromToken);
            var items = ObjectMapper.Map<List<PatientProvider>, List<PatientProviderDto>>(listOfUserPharmacies);
            foreach (var item in items)
            {
                if (item.Id == input.ProviderId)
                {
                    var oldPharmacy = listOfUserPharmacies.Find(ph => ph.ProviderId == input.ProviderId);
                    oldPharmacy.AddingDate = DateTime.Now;
                    return _apiResponse.Success(true);
                }
            }
            PatientProvider patientProvider = new PatientProvider(GuidGenerator.Create(), patient.Id, input.ProviderId, DateTime.Now, 0);
            await _patientProviderRepository.InsertAsync(patientProvider);
            return _apiResponse.Success(true);
        }

        [Authorize]
        public async Task<Response<PagedResultDto<PharmacyInfoDto>>> GetMyPharmacies(GetPatientOfProviderForMobile input)
        {
            var currentUserFromToken = _currentPatient.getUserNameFromToken();
            var totalCount = await _patientProviderRepository.GetCountForMobileAsync(currentUserFromToken, input.FilterText, input.PharmacyName, input.PharmacyPhone);
            var items = await _patientProviderRepository.GetListForMobileAsync(currentUserFromToken, input.FilterText,
                input.PharmacyName, input.PharmacyPhone, input.Sorting, input.MaxResultCount, input.SkipCount);
            var data = new PagedResultDto<PharmacyInfoDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<PatientProvider>, List<PharmacyInfoDto>>(items)
            };

            return _apiResponse.Success<PagedResultDto<PharmacyInfoDto>>(data);
        }

        [Authorize]
        public async Task<Response<PharmacyInfoDto>> GetProviderAsync(Guid id)
        {
            var currentUserFromToken = _currentPatient.getUserNameFromToken();
            var provider = await _patientProviderRepository.GetProviderForMobileAsync(id, currentUserFromToken);
            if (provider == null) throw new UserFriendlyException("Not Found");
            var providerDto = ObjectMapper.Map<PatientProvider, PharmacyInfoDto>(provider);
            return _apiResponse.Success<PharmacyInfoDto>(providerDto);
        }

        #endregion
    }
}
