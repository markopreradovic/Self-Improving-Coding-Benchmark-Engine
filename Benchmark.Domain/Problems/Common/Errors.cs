using ErrorOr;

namespace Benchmark.Domain.Problems.Common;

public static partial class DomainErrors
{
    public static class Problem
    {
        public static readonly Error InvalidTitle = Error.Validation("Problem.InvalidTitle", "Title cannot be empty.");
        public static readonly Error InvalidDescription = Error.Validation("Problem.InvalidDescription", "Description cannot be empty.");
        public static readonly Error InvalidCategory = Error.Validation("Problem.InvalidCategory", "Invalid category specified.");
        public static readonly Error NoTestCases =
            Error.Validation("Problem.NoTestCases", "At least one test case is required.");
    }
}