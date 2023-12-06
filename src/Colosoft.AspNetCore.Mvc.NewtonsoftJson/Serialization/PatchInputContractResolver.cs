using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Colosoft.AspNetCore.Mvc.Serialization
{
    public class PatchInputContractResolver : DefaultContractResolver
    {
        public PatchInputContractResolver()
        {
            this.NamingStrategy = new CamelCaseNamingStrategy();
        }

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            var valueProvider = base.CreateMemberValueProvider(member);
            valueProvider = new PatchInputValueProvider(valueProvider, member.Name);
            return valueProvider;
        }
    }
}
