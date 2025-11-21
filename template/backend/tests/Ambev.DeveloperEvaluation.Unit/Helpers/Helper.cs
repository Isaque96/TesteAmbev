using AutoMapper;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Helpers;

public static class Helper
{
    public static IMapper CreateMapper()
    {
        var application = Assembly.Load("Ambev.DeveloperEvaluation.Application");
        var web =  Assembly.Load("Ambev.DeveloperEvaluation.WebApi");
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(application);
            cfg.AddMaps(web);
        });

        return config.CreateMapper();
    }
}