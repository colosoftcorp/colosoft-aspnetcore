using System;
using System.Text.RegularExpressions;

namespace Colosoft.AspNetCore.Mvc
{
    /// <summary>
    /// The rules of a sorting text: each field is a plain identifier (letters, digits and underscore, not starting
    /// with a digit, dotted for a prefix) and each direction is <c>asc</c> or <c>desc</c>.
    /// </summary>
    public static class SortingSyntax
    {
        public const string AscendingTerm = "asc";
        public const string DescendingTerm = "desc";

        // ASCII classes only and no RegexOptions.IgnoreCase (it lets the Kelvin sign match 'k'); \z instead of $,
        // which would also accept a trailing line feed.
        private static readonly Regex FieldNamePattern = new Regex(
            @"\A[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z_][A-Za-z0-9_]*)*\z",
            RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(100));

        private static readonly char[] TokenSeparators = new[] { ' ', ':' };

        /// <summary>
        /// Tells whether the name is a plain identifier that can be a sorting field.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <returns><see langword="true"/> when the name is valid.</returns>
        public static bool IsValidFieldName(string name) =>
            !string.IsNullOrEmpty(name) && FieldNamePattern.IsMatch(name);

        /// <summary>
        /// Reads one item of a sorting text: <c>field</c>, <c>field asc</c>, <c>field desc</c>, <c>field:asc</c> or
        /// <c>field:desc</c>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="name">The field name.</param>
        /// <param name="isAscending">Whether the direction is ascending.</param>
        /// <exception cref="SortingFormatException">The item is not valid.</exception>
        public static void ParseItem(string item, out string name, out bool isAscending)
        {
            var tokens = (item ?? string.Empty).Split(TokenSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length < 1 || tokens.Length > 2)
            {
                throw new SortingFormatException("The sorting is not valid. Use field, field:asc or field:desc.");
            }

            name = tokens[0];
            EnsureValidFieldName(name);
            isAscending = tokens.Length == 1 || ParseDirection(tokens[1]);
        }

        /// <summary>
        /// Reads a direction.
        /// </summary>
        /// <param name="direction">The direction text.</param>
        /// <returns><see langword="true"/> for <c>asc</c>, <see langword="false"/> for <c>desc</c>.</returns>
        /// <exception cref="SortingFormatException">The direction is neither <c>asc</c> nor <c>desc</c>.</exception>
        public static bool ParseDirection(string direction)
        {
            if (string.Equals(direction, AscendingTerm, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(direction, DescendingTerm, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            throw new SortingFormatException("The sorting direction is not valid. Use asc or desc.");
        }

        /// <summary>
        /// Refuses a field name that is not a plain identifier.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <exception cref="SortingFormatException">The name is not valid.</exception>
        public static void EnsureValidFieldName(string name)
        {
            if (!IsValidFieldName(name))
            {
                throw new SortingFormatException(
                    "The sorting field is not valid. A field has only letters, digits, underscore and dots.");
            }
        }
    }
}
