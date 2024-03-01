using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
             var mapping=new MapperConfiguration(config =>
             {
                 config.CreateMap<Product,ProductDto>().ReverseMap();
             });
            return mapping;
        }
    }
}
