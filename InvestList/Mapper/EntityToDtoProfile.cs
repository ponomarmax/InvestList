using AutoMapper;
using Core.Entities;
using InvestList.Services.Queries;
using Radar.Application;
using Radar.Application.Models;
using Radar.Domain.Entities;

namespace InvestList.Mapper;

public class EntityToDtoProfile:Profile
{
    public EntityToDtoProfile()
    {
        CreateMap<InvestPost, InvestPostShortDto>();
        CreateMap<InvestPost, InvestPostShortDto>();

    }
}