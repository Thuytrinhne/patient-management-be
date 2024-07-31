namespace PatientManagementApi.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PaginationResult<Patient>, PaginationResult<GetPatientsResponseDto>>().ReverseMap();
            CreateMap<Patient, CreatePatientRequestDto>().ReverseMap();
            CreateMap<GetContactInforDto, ContactInfor>().ReverseMap();
            CreateMap<CreateContactInforDto, ContactInfor>().ReverseMap();
            CreateMap<UpdateContactInforDto, ContactInfor>().ReverseMap();

            CreateMap<GetAddressDto, Address>().ReverseMap();
            CreateMap<CreateAddressDto, Address>().ReverseMap();
            CreateMap<UpdateAddressDto, Address>().ReverseMap();

            CreateMap<Patient, GetPatientsResponseDto>().ReverseMap();
            CreateMap<Patient, UpdatePatientRequestDto>().ReverseMap();


        }
    }
}
