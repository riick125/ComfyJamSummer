using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace ComfyJamSummer.AutoMapper
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper;

        public static IMapper RegisterMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToJsonMappingProfile());
                cfg.AddProfile(new EntityToCustomEntityMappingProfile());
                cfg.AddProfile(new JsonToEntityMappingProfile());
            }, NullLoggerFactory.Instance);

            return config.CreateMapper();
        }
    }
}