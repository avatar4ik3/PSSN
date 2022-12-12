using AutoMapper;
using PSSN.Api.Model;
using PSSN.Common;
using PSSN.Common.Model;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Api.Profiles;

public class VectorProfile : Profile
{
    public VectorProfile()
    {
        //strat -> string
        CreateMap<IStrategy, string>().ConvertUsing(strat => strat.Name);

        // <strat,double> -> {string,double}
        CreateMap<
            KeyValuePair<IStrategy, double>,
            KeyValuePair<String, double>>()
                .ConstructUsing((source, context) => new KeyValuePair<string, double>(
                        context.Mapper.Map<string>(source.Key), source.Value
                    ));

        // inner answer -> outer answer

        //todo какое-то говно если вот честно
        CreateMap<ResultVector, VectorResponse>()
            .ForMember(x => x.Ki, m => m.MapFrom(y => y.Stage))
            .AfterMap((x, y, context) =>
                y.Values = new(
                    x.Vector
                        .Select(kvp =>
                            context.Mapper.Map<KeyValuePair<string, double>>(kvp)).ToArray()));

        CreateMap<TreeGameRunnerResult, ResultTree>()
            .ForMember(x => x.Map, m => m.MapFrom(y => y.map));

        CreateMap<FilledStrategy, FilledStrategyModel>()
            .ForMember(x => x.Name, m => m.MapFrom(y => y.Name))
            .ForMember(x => x.Behaviors, m => m.MapFrom(y => y.behaviours));

    }
}