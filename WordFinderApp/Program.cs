using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordFinderApp.Core.Validation;

class Program
{
    static void Main(string[] args)
    {
        // Configurar servicios
        var serviceProvider = new ServiceCollection()
            .AddLogging(config => config.AddConsole())
            .AddSingleton<IRepository, Repository>()
            .AddSingleton<ISearchStrategy, HorizontalSearchStrategy>()
            .AddSingleton<ISearchStrategy, VerticalSearchStrategy>()
            .AddSingleton<IValidator<IRepository>,WordFinderValidator>()
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<ILogger<Program>>();

        try
        {
            var wordFinder = FactoryWordFinder.Create(serviceProvider);

            var wordStream = new List<string> { "cold", "chill", "wind" };

            var foundWords =  wordFinder.FindAsync(wordStream).Result;

            Console.WriteLine("Words found:");
            foreach (var word in foundWords)
            {
                Console.WriteLine(word);
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while running the program.");
        }
    }
}
