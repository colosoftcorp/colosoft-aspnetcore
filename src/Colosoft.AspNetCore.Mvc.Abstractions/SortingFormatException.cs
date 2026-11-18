using System;

namespace Colosoft.AspNetCore.Mvc
{
    /// <summary>
    /// The sorting text names a field that is not a plain identifier or a direction that is not <c>asc</c> or
    /// <c>desc</c>. The sorting model binder turns it into a model state error (<c>400</c>).
    /// </summary>
    [Serializable]
    public class SortingFormatException : FormatException
    {
        public SortingFormatException()
        {
        }

        public SortingFormatException(string message)
            : base(message)
        {
        }

        public SortingFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
