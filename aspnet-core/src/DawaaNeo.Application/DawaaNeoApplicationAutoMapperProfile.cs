using AutoMapper;
using DawaaNeo.Orders;
using DawaaNeo.Patients;
using DawaaNeo.Providers.Mobile;
using DawaaNeo.Providers.valueObject;
using DawaaNeo.Providers;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using DawaaNeo.Localization;
using System.Globalization;
using DawaaNeo.Services;

namespace DawaaNeo;

public class DawaaNeoApplicationAutoMapperProfile : Profile , ISingletonDependency
{
    private readonly IStringLocalizer<DawaaNeoResource> L;

    public DawaaNeoApplicationAutoMapperProfile(IStringLocalizer<DawaaNeoResource> l)
    {
        L = l;
        var currentLanguage = CultureInfo.CurrentCulture;

        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<WorkingTime, WorkingTimeDto>().ReverseMap();
        CreateMap<Provider, ProviderDto>()
            .ForMember(dest => dest.WorkingTimes, opt => opt.MapFrom(src => src.WorkingTimes != null ? src.WorkingTimes : null));
        CreateMap<ProviderAddress, ProviderAddressDto>().ReverseMap();
        CreateMap<PatientProvider, PatientProviderDto>()
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.MobileNumber : ""))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.CountryCode : ""));


        CreateMap<WorkingTime, WorkingTimeForMobileDto>();
        CreateMap<PatientProvider, PharmacyInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Provider.Id != null ? src.Provider.Id : new System.Guid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Provider.PharmacyName != null ? src.Provider.PharmacyName : null))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Provider.PharmacyPhone != null ? src.Provider.PharmacyPhone : null))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Provider.LocationInfo.Longitude != null ? src.Provider.LocationInfo.Longitude : 0))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Provider.LocationInfo.Latitude != null ? src.Provider.LocationInfo.Latitude : 0))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Provider.LocationInfo.Address != null ? src.Provider.LocationInfo.Address : null))
            .ForMember(dest => dest.AddingDate, opt => opt.MapFrom(src => src.AddingDate != null ? src.AddingDate : System.DateTime.Now))
            .ForMember(dest => dest.WorkingTimes, opt => opt.MapFrom(src => src.Provider.WorkingTimes != null ? src.Provider.WorkingTimes : null));


        CreateMap<PatientAddress, PatientAddressDto>().ReverseMap();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Currency , opt => opt.MapFrom(src => L[src.Currency]))
            .ReverseMap();

        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<DawaaNeo.Services.Service, ServiceDto>().ReverseMap();
    }
}
