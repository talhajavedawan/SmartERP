using AutoMapper;
using ERP_BL.Entities;

namespace ERP_REPO.Repo
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyGetDto>()
                .ForMember(dest => dest.CountryName,
                    opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null));

            CreateMap<CurrencyCreateDto, Currency>();
            CreateMap<CurrencyUpdateDto, Currency>();
        }
    }
}
