namespace Colosoft.AspNetCore.Mvc.Test
{
    internal static class MaliciousSortings
    {
        public static TheoryData<string> All => new TheoryData<string>
        {
            "(SELECT SLEEP(3))",
            "name;UPDATE x SET y=1#",
            "name;UPDATE x SET y=1 --",
            "CASE WHEN (1=1) THEN name ELSE id END",
            "name -- comment",
            "name #comment",
            "name /* comment */",
            "`name`",
            "'name'",
            "\"name\"",
            "[name]",
            "name:asc;DROP TABLE x",
            "name:sleep(3)",
            "name (SELECT 1)",
            "name asc desc",
            "1name",
            "name.",
            "a..b",
            "naKe",
        };
    }
}
