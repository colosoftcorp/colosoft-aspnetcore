namespace Colosoft.AspNetCore.Mvc.Test
{
    public class BaseSortingTranslationTest
    {
        public static TheoryData<string> MaliciousSortings => Test.MaliciousSortings.All;

        [Theory]
        [InlineData("name", "c.Name asc")]
        [InlineData("name desc", "c.Name desc")]
        [InlineData("name:DESC", "c.Name desc")]
        [InlineData("name asc,id desc", "c.Name asc")]
        [InlineData("id desc", "c.Id desc")]
        [InlineData("", "c.Id desc")]
        public void TranslateKeepsOnlyAscOrDesc(string sorting, string expected)
        {
            Assert.Equal(expected, new NameSortingTranslation(sorting).Translate());
        }

        [Theory]
        [MemberData(nameof(MaliciousSortings))]
        [InlineData("name up")]
        public void TranslateRefusesInvalidFieldsAndDirections(string sorting)
        {
            Assert.Throws<SortingFormatException>(() => new NameSortingTranslation(sorting).Translate());
        }
    }
}
