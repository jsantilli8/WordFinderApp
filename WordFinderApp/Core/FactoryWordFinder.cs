using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class FactoryWordFinder
{
    public static WordFinder Create(IServiceProvider serviceProvider)
    {
        var repository = serviceProvider.GetRequiredService<IRepository>();
        var strategies = serviceProvider.GetRequiredService<IEnumerable<ISearchStrategy>>();
        var logger = serviceProvider.GetRequiredService<ILogger<WordFinder>>();
        var validator = serviceProvider.GetRequiredService<IValidator<IRepository>>();
        return new WordFinder(repository, strategies, logger,validator);
    }
}
