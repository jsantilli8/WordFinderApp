using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;

namespace WordFinderApp.Core.Validation
{
    public class WordFinderValidator : AbstractValidator<IRepository>
    {
        public WordFinderValidator()
        {
         RuleFor(repo => repo.GetMatrix())
            .NotNull().WithMessage("Matrix can not be null.")
            .Must(matrix => matrix.Any()).WithMessage("Matrix can not be empty.")
            .Must(matrix => matrix.All(row => row.Length == matrix.First().Length))
            .WithMessage("All rows in the matrix must have the same number of characters.")
            .Must(matrix => matrix.Count() <= 64 && matrix.First().Length <= 64)
            .WithMessage("Matrix dimensions can not exceed 64x64.");

        }
    }
}
