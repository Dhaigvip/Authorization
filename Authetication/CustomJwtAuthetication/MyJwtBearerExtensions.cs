using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CustomAuthetication
{
    public static class MyJwtBearerExtensions
    {


        public static AuthenticationBuilder AddMyJwtBearer(this AuthenticationBuilder builder)
        {
            return AddMyJwtBearer(builder, MyJwtBearerDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddMyJwtBearer(this AuthenticationBuilder builder,
            string autheticationScheme)
        {
            return AddMyJwtBearer(builder, autheticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddMyJwtBearer(this AuthenticationBuilder builder,
            Action<MyJwtBearerOptions> configureOptions)
        {
            return AddMyJwtBearer(builder, MyJwtBearerDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddMyJwtBearer(this AuthenticationBuilder builder,
            string autheticationScheme, Action<MyJwtBearerOptions> configureOptions)
        {
            builder.Services
                .AddSingleton<IPostConfigureOptions<MyJwtBearerOptions>,
                    MyJwtBearerPostConfigureOptions<MyJwtBearerOptions>>();

            return builder.AddScheme<MyJwtBearerOptions, MyJwtBearerHandler>(autheticationScheme,
                configureOptions);
        }
    }
}
