using System;

namespace Colosoft.AspNetCore.Mvc
{
    public interface ISortingTranslatorFactory
    {
        ISortingTranslator Create(Type translateType);
    }
}
