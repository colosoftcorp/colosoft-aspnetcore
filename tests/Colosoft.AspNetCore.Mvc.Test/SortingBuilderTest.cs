namespace Colosoft.AspNetCore.Mvc.Test
{
    public class SortingBuilderTest
    {
        public static TheoryData<string> MaliciousSortings => Test.MaliciousSortings.All;

        [Theory]
        [InlineData("name", "name asc")]
        [InlineData("name:desc", "name desc")]
        [InlineData("name:DESC", "name desc")]
        [InlineData("name desc", "name desc")]
        [InlineData("c.Name:asc, createdAt:desc", "c.Name asc,createdAt desc")]
        [InlineData("_name,", "_name asc")]
        [InlineData("", "")]
        public void AddExpressionReadsValidSortings(string expression, string expected)
        {
            var sorting = new SortingBuilder().AddExpression(expression).Build();

            Assert.Equal(expected, sorting.ToString());
        }

        [Theory]
        [MemberData(nameof(MaliciousSortings))]
        [InlineData("name:up")]
        public void AddExpressionRefusesMaliciousSortings(string expression)
        {
            Assert.Throws<SortingFormatException>(() => new SortingBuilder().AddExpression(expression));
        }

        [Theory]
        [InlineData("name desc")]
        [InlineData("(SELECT 1)")]
        [InlineData("")]
        public void AddFieldRefusesNamesThatAreNotIdentifiers(string name)
        {
            Assert.Throws<SortingFormatException>(() => new SortingBuilder().AddField(name, true));
        }
    }
}
