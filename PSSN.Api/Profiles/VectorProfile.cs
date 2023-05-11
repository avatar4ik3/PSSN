using AutoMapper;
using PSSN.Api.Controllers;
using PSSN.Api.Model;
using PSSN.Common;
using PSSN.Common.Model;
using PSSN.Common.Responses;
using PSSN.Core.Round;
using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;

namespace PSSN.Api.Profiles;

public class VectorProfile : Profile
{
    public VectorProfile()
    {
        //strat -> string
        CreateMap<IStrategy, string>().ConvertUsing(strat => strat.Name);

        CreateMap<IStrategy, TreeId>()
            .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
            .ForMember(x => x.Name, m => m.MapFrom(y => y.Name));
        // <strat,double> -> {string,double}
        CreateMap<
                KeyValuePair<IStrategy, double>,
                KeyValuePair<string, double>>()
            .ConstructUsing((source, context) => new KeyValuePair<string, double>(
                context.Mapper.Map<string>(source.Key), source.Value
            ));
        CreateMap<
                KeyValuePair<IStrategy, double>,
                KeyValuePair<TreeId, double>>()
            .ConstructUsing((source, context) => new KeyValuePair<TreeId, double>(
                context.Mapper.Map<TreeId>(source.Key), source.Value
            ));

        // inner answer -> outer answer

        //todo какое-то говно если вот честно
        CreateMap<ResultVector, VectorResponse>()
            .ForMember(x => x.Ki, m => m.MapFrom(y => y.Stage))
            .AfterMap((x, y, context) =>
                y.Values = new Dictionary<string, double>(
                    x.Vector
                        .Select(kvp =>
                            context.Mapper.Map<KeyValuePair<string, double>>(kvp)).ToArray()));

        CreateMap<TreeGameRunnerResult, ResultTree>()
            .ForMember(x => x.Map, m => m.MapFrom(y => y.map));

        CreateMap<ConditionalStrategy, ConditionalStrategyModel>()
            .ForMember(x => x.Name, m => m.MapFrom(y => y.Name))
            .ForMember(x => x.Id,m=>m.MapFrom(y => y.Id))
            .ForMember(x => x.Behaviors, m => m.MapFrom(y => y.Behaviours))
            .ForMember(x => x.Pattern, m => m.MapFrom(y => y.Pattern));


        CreateMap<IBehaviourPattern, PatternModel>()
            .ForMember(x => x.Name, m => m.MapFrom(y => y.GetType().Name))
            .ForMember(x => x.Coeffs, m => m.MapFrom(y => y.Coeffs));

        //TODO возможно придется убрать эту мапу, потому что поведения тут не мапятся
        CreateMap<ConditionalStrategyModel, ConditionalStrategy>()
            .ForMember(x => x.Name, m => m.MapFrom(y => y.Name))
            .ForMember(x => x.Behaviours, m => m.MapFrom(y => y.Behaviors))
            .ForMember(x => x.Pattern, m => m.Ignore());

        CreateMap<IntWrapper,int>().ConstructUsing((val,ctx) => val.Value);
    }
}