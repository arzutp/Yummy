using AutoMapper;
using Yummy.WebApi.Dtos.CategoryDtos;
using Yummy.WebApi.Dtos.ChefDtos;
using Yummy.WebApi.Dtos.ContactDtos;
using Yummy.WebApi.Dtos.FeatureDtos;
using Yummy.WebApi.Dtos.MessageDtos;
using Yummy.WebApi.Dtos.ProductDtos;
using Yummy.WebApi.Dtos.ServiceDtos;
using Yummy.WebApi.Dtos.SpecialEventDtos;
using Yummy.WebApi.Dtos.TestimonialDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
        CreateMap<Category, GetByIdCategoryDto>().ReverseMap();
        CreateMap<Category, ResultCategoryDto>().ReverseMap();
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();

        CreateMap<Chef, CreateChefDto>().ReverseMap();
        CreateMap<Chef, GetByIdChefDto>().ReverseMap();
        CreateMap<Chef, ResultChefDto>().ReverseMap();
        CreateMap<Chef, UpdateChefDto>().ReverseMap();

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

        CreateMap<Service, CreateServiceDto>().ReverseMap();
        CreateMap<Service, GetByIdServiceDto>().ReverseMap();
        CreateMap<Service, ResultServiceDto>().ReverseMap();
        CreateMap<Service, UpdateServiceDto>().ReverseMap();

        CreateMap<SpecialEvent, CreateSpecialEventDto>().ReverseMap();
        CreateMap<SpecialEvent, GetByIdSpecialEventDto>().ReverseMap();
        CreateMap<SpecialEvent, ResultSpecialEventDto>().ReverseMap();
        CreateMap<SpecialEvent, UpdateSpecialEventDto>().ReverseMap();

        CreateMap<Testimonial, CreateTestimonialDto>().ReverseMap();
        CreateMap<Testimonial, GetByIdTestimonialDto>().ReverseMap();
        CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();
        CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();
    }
}
