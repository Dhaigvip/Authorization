using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomAuthetication
{
    public class MyJwtBearerPostConfigureOptions<MyJwtBearerOptions> : JwtBearerPostConfigureOptions
    {
        public void PostConfigure(string name, MyJwtBearerOptions options)
        {
            base.PostConfigure(name, options as JwtBearerOptions);
        }
    }
}
