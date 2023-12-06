using Colosoft.Input;
using Newtonsoft.Json.Serialization;

namespace Colosoft.AspNetCore.Mvc.Serialization
{
    internal class PatchInputValueProvider : IValueProvider
    {
        private readonly IValueProvider wrapper;
        private readonly string memberName;

        public PatchInputValueProvider(IValueProvider wrapper, string memberName)
        {
            this.wrapper = wrapper;
            this.memberName = memberName;
        }

        public object GetValue(object target) =>
            this.wrapper.GetValue(target);

        public void SetValue(object target, object value)
        {
            this.wrapper.SetValue(target, value);

            var patchInput = target as IPatchInput;

            if (patchInput != null)
            {
                patchInput.AddChange(this.memberName);
            }
        }
    }
}
