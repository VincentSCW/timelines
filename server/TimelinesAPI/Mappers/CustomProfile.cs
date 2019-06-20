using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Models;

namespace TimelinesAPI.Mappers
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<TimelineEntity, TimelineModel>()
                .ForMember(x => x.Username, opt => opt.MapFrom(y => y.PartitionKey))
                .ForMember(x => x.TopicKey, opt => opt.MapFrom(y => y.RowKey))
                .ReverseMap();

            CreateMap<MomentEntity, MomentModel>()
                .ForMember(x => x.TopicKey, opt => opt.MapFrom(y => y.PartitionKey))
                .ForMember(x => x.RecordDate, opt => opt.MapFrom(y => y.RowKey))
                .ReverseMap();

            CreateMap<RecordEntity, RecordModel>()
                .ForMember(x => x.Key, opt => opt.MapFrom(y => y.RowKey))
                .ReverseMap();
        }
    }
}
