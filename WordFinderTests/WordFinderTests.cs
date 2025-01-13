// WordFinderTests class: From simple to complex test cases
using Microsoft.Extensions.Logging;
using Moq;
using WordFinderApp.Core.Validation;

[TestClass]
public class WordFinderTests
{
    private Mock<IRepository> _mockRepository;
    private Mock<ILogger<WordFinder>> _mockLogger;
    private List<ISearchStrategy> _mockStrategies;
    private WordFinderValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<WordFinder>>();
        _mockStrategies = new List<ISearchStrategy>
        {
            new HorizontalSearchStrategy(),
            new VerticalSearchStrategy()
        };
        _validator = new WordFinderValidator();
    }

    // Simple Tests

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() =>
            new WordFinder(null, _mockStrategies, _mockLogger.Object, _validator));
    }

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenStrategiesAreNull()
    {
        // Arrange, Act & Assert
        Assert.ThrowsException<ArgumentException>(() =>
            new WordFinder(_mockRepository.Object, null, _mockLogger.Object, _validator));
    }

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() =>
            new WordFinder(_mockRepository.Object, _mockStrategies, null, _validator));
    }

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenMatrixExceeds64x64()
    {
        // Arrange
        var largeMatrix = Enumerable.Repeat(new string('a', 65), 65).ToList();
        _mockRepository.Setup(r => r.GetMatrix()).Returns(largeMatrix);

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() =>
            new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator));
    }

    [TestMethod]
    public void Constructor_ShouldInitializeSuccessfully_WithValidInputs()
    {
        // Arrange
        var validMatrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        _mockRepository.Setup(r => r.GetMatrix()).Returns(validMatrix);

        // Act
        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Assert
        Assert.IsNotNull(wordFinder);
    }

    // Intermediate Tests

    [TestMethod]
    public async Task FindAsync_ShouldReturnEmpty_WhenWordStreamIsEmpty()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string>();

        _mockRepository.Setup(r => r.GetMatrix()).Returns(matrix);

        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task FindAsync_ShouldReturnWords_WhenWordsExistInMatrix()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string> { "abcd", "ijkl", "mnop" };

        _mockRepository.Setup(r => r.GetMatrix()).Returns(matrix);

        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        CollectionAssert.AreEqual(new List<string> { "abcd", "ijkl", "mnop" }, result.ToList());
    }

    [TestMethod]
    public async Task FindAsync_ShouldReturnTop10Words_WhenMoreThan10WordsProvided()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = Enumerable.Range(1, 20).Select(i => $"word{i}").ToList();

        _mockRepository.Setup(r => r.GetMatrix()).Returns(matrix);

        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        Assert.AreEqual(10, result.Count());
    }

    // Complex Tests

    [TestMethod]
    public async Task FindAsync_ShouldWorkWithLargeWordStream()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = Enumerable.Repeat("word", 100).ToList();

        _mockRepository.Setup(r => r.GetMatrix()).Returns(matrix);

        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        Assert.IsTrue(result.Count() <= 10);
    }

    [TestMethod]
    public async Task FindAsync_ShouldReturnEmpty_WhenNoWordsMatch()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string> { "xyz", "unknown" };

        _mockRepository.Setup(r => r.GetMatrix()).Returns(matrix);

        var wordFinder = new WordFinder(_mockRepository.Object, _mockStrategies, _mockLogger.Object, _validator);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        Assert.AreEqual(0, result.Count());
    }
}
