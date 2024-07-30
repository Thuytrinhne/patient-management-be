using AutoMapper;

namespace PatientManagementApi.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PaginationResult<Patient>, PaginationResult<GetPatientsResponseDto>>().ReverseMap();
        }
    }
}
