using Colosoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Colosoft.AspNetCore.Mvc.Test
{
    public class SortingModelBinderTest
    {
        public static TheoryData<string> MaliciousSortings => Test.MaliciousSortings.All;

        [Theory]
        [InlineData("name:desc", "name desc")]
        [InlineData("name, createdAt:desc", "name asc,createdAt desc")]
        [InlineData(null, "")]
        public async Task BindsValidSortings(string? sort, string expected)
        {
            var bindingContext = this.CreateBindingContext(nameof(SampleController.WithoutTranslation), sort);

            await new SortingModelBinder().BindModelAsync(bindingContext);

            Assert.True(bindingContext.Result.IsModelSet);
            Assert.True(bindingContext.ModelState.IsValid);
            Assert.Equal(expected, bindingContext.Result.Model!.ToString());
        }

        [Theory]
        [MemberData(nameof(MaliciousSortings))]
        public async Task RefusesMaliciousSortingsWithAModelStateError(string sort)
        {
            var bindingContext = this.CreateBindingContext(nameof(SampleController.WithoutTranslation), sort);

            await new SortingModelBinder().BindModelAsync(bindingContext);

            Assert.False(bindingContext.Result.IsModelSet);
            Assert.False(bindingContext.ModelState.IsValid);
            Assert.True(bindingContext.ModelState.ContainsKey("sort"));
        }

        [Theory]
        [InlineData(nameof(SampleController.WithTranslationOnMethod))]
        [InlineData(nameof(SampleController.WithTranslationOnParameter))]
        public async Task AppliesTheTranslationOfTheAction(string action)
        {
            var bindingContext = this.CreateBindingContext(action, "name:desc");

            await new SortingModelBinder().BindModelAsync(bindingContext);

            Assert.True(bindingContext.Result.IsModelSet);
            Assert.Equal("c.Name desc", bindingContext.Result.Model!.ToString());
        }

        [Fact]
        public async Task AppliesTheDefaultSortingOfTheTranslation()
        {
            var bindingContext = this.CreateBindingContext(nameof(SampleController.WithTranslationOnMethod), null);

            await new SortingModelBinder().BindModelAsync(bindingContext);

            Assert.Equal("c.Id desc", bindingContext.Result.Model!.ToString());
        }

        [Fact]
        public void TheTranslationAttributeCanBeOnMethodsAndParameters()
        {
            var usage = typeof(SortingTranslationAttribute).GetCustomAttribute<AttributeUsageAttribute>() !;

            Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Method));
            Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Parameter));
        }

        private DefaultModelBindingContext CreateBindingContext(string action, string? sort)
        {
            var services = new ServiceCollection();
            services.AddColosoftMvc();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider(),
            };

            if (sort != null)
            {
                httpContext.Request.QueryString = QueryString.Create("sort", sort);
            }

            var method = typeof(SampleController).GetMethod(action) !;
            var parameterInfo = method.GetParameters()[0];

            var actionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = method,
                ControllerTypeInfo = typeof(SampleController).GetTypeInfo(),
                Parameters = new List<Microsoft.AspNetCore.Mvc.Abstractions.ParameterDescriptor>
                {
                    new ControllerParameterDescriptor
                    {
                        Name = parameterInfo.Name!,
                        ParameterType = parameterInfo.ParameterType,
                        ParameterInfo = parameterInfo,
                    },
                },
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), actionDescriptor, new ModelStateDictionary());
            var metadata = new EmptyModelMetadataProvider().GetMetadataForParameter(parameterInfo);
            var valueProvider = new QueryStringValueProvider(BindingSource.Query, httpContext.Request.Query, CultureInfo.InvariantCulture);

            return (DefaultModelBindingContext)DefaultModelBindingContext.CreateBindingContext(
                actionContext,
                valueProvider,
                metadata,
                new BindingInfo(),
                parameterInfo.Name!);
        }

        public class SampleController : ControllerBase
        {
            public IActionResult WithoutTranslation(ISorting sorting) => this.Ok(sorting);

            [SortingTranslation(typeof(NameSortingTranslation))]
            public IActionResult WithTranslationOnMethod(ISorting sorting) => this.Ok(sorting);

            public IActionResult WithTranslationOnParameter(
                [SortingTranslation(typeof(NameSortingTranslation))] ISorting sorting) => this.Ok(sorting);
        }
    }
}
