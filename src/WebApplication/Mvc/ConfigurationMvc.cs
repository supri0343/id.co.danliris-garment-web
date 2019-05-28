using DanLiris.Admin.Web;
using ExtCore.Mvc.Infrastructure.Actions;
using FluentValidation.AspNetCore;
using Infrastructure.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;

namespace Infrastructure.Mvc
{
    public class ConfigurationMvc : IAddMvcAction
    {
        public int Priority => 1000;

        public void Execute(IMvcBuilder builder, IServiceProvider sp)
        {
            builder.AddMvcOptions(c =>
            {
                c.Filters.Add<TransactionDbFilter>();
                //c.Filters.Add(new ValidateModelStateFilter());
                c.Filters.Add<GlobalExceptionFilter>();
                c.Filters.Add<ValidateModelStateAttribute>();
            });
            builder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            builder.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
        }
    }
}