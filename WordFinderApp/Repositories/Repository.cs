using System.Collections.Generic;

public class Repository : IRepository
{
    public IEnumerable<string> GetMatrix()
    {
        return new List<string>
        {
            "abcdc",
            "fgwio",
            "chill",
            "pqnsd",
            "uvdxy"
        };
    }
}
