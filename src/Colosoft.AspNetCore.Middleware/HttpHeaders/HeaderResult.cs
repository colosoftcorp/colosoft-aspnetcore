namespace Colosoft.AspNetCore.Middleware.HttpHeaders
{
    public class HeaderResult
    {
        public enum ResponseAction
        {
            Set = 0,
            Remove = 1,
        }

        public HeaderResult(ResponseAction action, string name, string? value = null)
        {
            this.Action = action;
            this.Name = name;
            this.Value = value;
        }

        public ResponseAction Action { get; set; }

        public string Name { get; set; }

        public string? Value { get; set; }
    }
}