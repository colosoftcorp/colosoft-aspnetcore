namespace Colosoft.AspNetCore.Mvc
{
    internal class SortField : ISortField
    {
        public SortField(string name, bool isAscending)
        {
            this.Name = name;
            this.IsAscending = isAscending;
        }

        public string Name { get; }

        public bool IsAscending { get; }

        public override string ToString() =>
            string.Format("{0} {1}", this.Name, this.IsAscending ? "asc" : "desc");
    }
}
