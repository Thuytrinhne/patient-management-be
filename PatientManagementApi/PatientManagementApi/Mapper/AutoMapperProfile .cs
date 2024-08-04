namespace PatientManagementApi.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PaginationResult<Patient>, PaginationResult<GetPatientsResponseDto>>().ReverseMap();
            CreateMap<Patient, CreatePatientRequestDto>().ReverseMap();
            CreateMap<GetContactInforDto, ContactInfor>().ReverseMap();
            CreateMap<UpsertContactInforDto, ContactInfor>().ReverseMap();

            CreateMap<GetAddressDto, Address>().ReverseMap();
            CreateMap<UpsertAddressDto, Address>().ReverseMap();

            CreateMap<Patient, GetPatientsResponseDto>().ReverseMap();
            CreateMap<Patient, UpdatePatientRequestDto>().ReverseMap();
            CreateMap<Patient, GetPatientByIdResponseDto>().ReverseMap();

            

        }
    }
}
