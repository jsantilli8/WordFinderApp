using FluentValidation;
using Microsoft.Extensions.Logging;

public class WordFinder
{
    private readonly char[,] _matrix;
    private readonly int _rows;
    private readonly int _cols;
    private readonly IEnumerable<ISearchStrategy> _strategies;
    private readonly ILogger<WordFinder> _logger;

    public WordFinder(
        IRepository repository,
        IEnumerable<ISearchStrategy> strategies,
        ILogger<WordFinder> logger,
        IValidator<IRepository> validator)
    {
        if (repository == null)
            throw new ArgumentNullException(nameof(repository));
        if (strategies == null || !strategies.Any())
            throw new ArgumentException("At least one search strategy must be provided.");
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));

        var validationResult = validator.Validate(repository);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var matrixList = repository.GetMatrix()?.ToList();
        _rows = matrixList.Count;
        _cols = matrixList[0].Length;

        _matrix = new char[_rows, _cols];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                _matrix[i, j] = matrixList[i][j];
            }
        }

        _strategies = strategies;
        _logger = logger;
        _logger.LogInformation("WordFinder initialized successfully.");
    }

    public async Task<IEnumerable<string>> FindAsync(IEnumerable<string> wordstream)
    {
        if (wordstream == null)
            throw new ArgumentNullException(nameof(wordstream));

        return await Task.Run(() =>
        {
            var foundWords = new HashSet<string>();

            foreach (var word in wordstream.Distinct())
            {
                if (ExistsInMatrix(word))
                {
                    foundWords.Add(word);
                    _logger.LogInformation($"Word found: {word}");
                }
            }

            return foundWords.OrderBy(w => w).Take(10);
        });
    }

    private bool ExistsInMatrix(string word)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                foreach (var strategy in _strategies)
                {
                    if (strategy.Search(_matrix, word, i, j))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
