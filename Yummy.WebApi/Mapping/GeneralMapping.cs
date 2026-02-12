using AutoMapper;
using Yummy.WebApi.Dtos.AboutDtos;
using Yummy.WebApi.Dtos.CategoryDtos;
using Yummy.WebApi.Dtos.ChefDtos;
using Yummy.WebApi.Dtos.ChefEmployeeTaskDtos;
using Yummy.WebApi.Dtos.ContactDtos;
using Yummy.WebApi.Dtos.EmployeeTasksDtos;
using Yummy.WebApi.Dtos.FeatureDtos;
using Yummy.WebApi.Dtos.ImageDtos;
using Yummy.WebApi.Dtos.MessageDtos;
using Yummy.WebApi.Dtos.NotificationDtos;
using Yummy.WebApi.Dtos.ProductDtos;
using Yummy.WebApi.Dtos.ReservationDtos;
using Yummy.WebApi.Dtos.ServiceDtos;
using Yummy.WebApi.Dtos.SpecialEventDtos;
using Yummy.WebApi.Dtos.TestimonialDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<About, CreateAboutDto>().ReverseMap();
        CreateMap<About, GetByIdAboutDto>().ReverseMap();
        CreateMap<About, ResultAboutDto>().ReverseMap();
        CreateMap<About, UpdateAboutDto>().ReverseMap();

        CreateMap<Category, CreateCategoryDto>().ReverseMap();
        CreateMap<Category, GetByIdCategoryDto>().ReverseMap();
        CreateMap<Category, ResultCategoryDto>().ReverseMap();
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();

        CreateMap<Chef, CreateChefDto>().ReverseMap();
        CreateMap<Chef, GetByIdChefDto>().ReverseMap();
        CreateMap<Chef, ResultChefDto>().ReverseMap();
        CreateMap<Chef, UpdateChefDto>().ReverseMap();

        CreateMap<ChefEmployeeTask, CreateChefEmployeeTaskDto>().ReverseMap();
        CreateMap<ChefEmployeeTask, GetByIdChefEmployeeTaskDto>().ReverseMap();
        CreateMap<ChefEmployeeTask, ResultChefEmployeeTaskDto>()
            .ForMember(x => x.ChefName,
                    opt => opt.MapFrom(src => src.Chef.NameSurname))
            .ForMember(x => x.TaskName,
                    opt => opt.MapFrom(src => src.EmployeeTasks.Name))
            .ForMember(x => x.TaskPriority,
                    opt => opt.MapFrom(src => src.EmployeeTasks.Priority));

        CreateMap<ChefEmployeeTask, UpdateChefEmployeeTaskDto>().ReverseMap();

        CreateMap<EmployeeTask, CreateEmployeeTaskDto>().ReverseMap();
        CreateMap<EmployeeTask, GetByIdEmployeeTaskDto>().ReverseMap();
        CreateMap<EmployeeTask, ResultEmployeeTaskDto>().ReverseMap();
        CreateMap<EmployeeTask, UpdateEmployeeTaskDto>().ReverseMap();

        CreateMap<Message, CreateMessageDto>().ReverseMap();
        CreateMap<Message, GetByIdMessageDto>().ReverseMap();
        CreateMap<Message, ResultMessageDto>().ReverseMap();

        CreateMap<Notification, CreateNotificationDto>().ReverseMap();
        CreateMap<Notification, GetByIdNotificationDto>().ReverseMap();
        CreateMap<Notification, ResultNotificationDto>().ReverseMap();
        CreateMap<Notification, UpdateNotificationDto>().ReverseMap();

        CreateMap<Feature, CreateFeatureDto>().ReverseMap();
        CreateMap<Feature, GetByIdFeatureDto>().ReverseMap();
        CreateMap<Feature, ResultFeatureDto>().ReverseMap();
        CreateMap<Feature, UpdateFeatureDto>().ReverseMap();

        CreateMap<Contact, CreateContactDto>().ReverseMap();
        CreateMap<Contact, GetByIdContactDto>().ReverseMap();
        CreateMap<Contact, ResultContactDto>().ReverseMap();
        CreateMap<Contact, UpdateContactDto>().ReverseMap();

        CreateMap<Image, CreateImageDto>().ReverseMap();
        CreateMap<Image, GetByIdImageDto>().ReverseMap();
        CreateMap<Image, ResultImageDto>().ReverseMap();
        CreateMap<Image, UpdateImageDto>().ReverseMap();

        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, GetByIdProductDto>().ReverseMap();
        CreateMap<Product, ResultProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();
        CreateMap<Product, ProductsWithCategoryDto>()
            .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name));

        CreateMap<Reservation, CreateReservationDto>().ReverseMap();
        CreateMap<Reservation, GetByIdReservationDto>().ReverseMap();
        CreateMap<Reservation, ResultReservationDto>().ReverseMap();
        CreateMap<Reservation, UpdateReservationDto>().ReverseMap();

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
