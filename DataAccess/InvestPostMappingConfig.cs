using Core.Entities;
using Mapster;
using Radar.Domain.Entities;

namespace DataAccess;

public class InvestPostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InvestPost, InvestPost>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PostId);
    }
}