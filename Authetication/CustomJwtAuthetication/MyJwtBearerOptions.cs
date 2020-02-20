using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomAuthetication
{
    public class MyJwtBearerOptions : JwtBearerOptions
    {
        public override void Validate()
        {
            base.Validate();
        }

        public override void Validate(string scheme)
        {
            base.Validate(scheme);
        }
        
    }
}
