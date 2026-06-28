using FluentAssertions;

namespace Blog.Post.Tests.Api;

public class DatabaseProviderRegressionTests
{
    [Theory]
    [MemberData(nameof(ForbiddenTexts))]
    public void Codigo_DeveUsarSomenteSqlServer(string forbiddenText)
    {
        var root = FindRepositoryRoot();
        var files = Directory
            .EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
            .Where(file => IsRelevantSource(file));

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            content.Should().NotContain(forbiddenText, $"arquivo {file} nao deve configurar banco diferente de SQL Server");
        }
    }

    public static IEnumerable<object[]> ForbiddenTexts()
    {
        yield return [string.Concat("Microsoft.EntityFrameworkCore.", "Sqli", "te")];
        yield return [string.Concat("Microsoft.EntityFrameworkCore.", "In", "Memory")];
        yield return [string.Concat("Use", "Sqli", "te")];
        yield return [string.Concat("Use", "In", "Memory", "Database")];
        yield return [string.Concat("Sqli", "te:")];
        yield return [string.Concat("Data", " Source=")];
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "CodingBlog.sln")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new InvalidOperationException("Nao foi possivel localizar a raiz do repositorio.");
    }

    private static bool IsRelevantSource(string file)
    {
        if (file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") ||
            file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") ||
            file.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") ||
            file.EndsWith($"{Path.DirectorySeparatorChar}DatabaseProviderRegressionTests.cs", StringComparison.Ordinal))
            return false;

        var extension = Path.GetExtension(file);
        return extension is ".cs" or ".csproj" or ".json";
    }
}
