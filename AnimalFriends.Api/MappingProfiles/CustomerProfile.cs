using AnimalFriends.Api.RequestModels;
using AnimalFriends.Application.Commands;
using AutoMapper;

namespace AnimalFriends.Api.MappingProfiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
    }
}