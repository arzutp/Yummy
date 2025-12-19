using AutoMapper;
using Yummy.WebApi.Dtos.ContactDtos;
using Yummy.WebApi.Dtos.FeatureDtos;
using Yummy.WebApi.Dtos.MessageDtos;
using Yummy.WebApi.Dtos.ProductDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Message, CreateMessageDto>().ReverseMap();
        CreateMap<Message, GetByIdMessageDto>().ReverseMap();
        CreateMap<Message, ResultMessageDto>().ReverseMap();

        CreateMap<Feature, CreateFeatureDto>().ReverseMap();
        CreateMap<Feature, GetByIdFeatureDto>().ReverseMap();
        CreateMap<Feature, ResultFeatureDto>().ReverseMap();
        CreateMap<Feature, UpdateFeatureDto>().ReverseMap();

        CreateMap<Contact, CreateContactDto>().ReverseMap();
        CreateMap<Contact, GetByIdContactDto>().ReverseMap();
        CreateMap<Contact, ResultContactDto>().ReverseMap();
        CreateMap<Contact, UpdateContactDto>().ReverseMap();
        
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, GetByIdProductDto>().ReverseMap();
        CreateMap<Product, ResultProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();
        CreateMap<Product, ProductsWithCategoryDto>()
            .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name));
    }
}
