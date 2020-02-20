using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CustomAuthetication
{
    public class RequirementOne : IAuthorizationRequirement
    {
        public RequirementOne()
        {

        }
    }

    public class RequirementOneHandler1 : AuthorizationHandler<RequirementOne>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirementOne requirement)
        {
            if (context.User.HasClaim("RequirementOne", "Ok"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    public class RequirementOneHandler2 : AuthorizationHandler<RequirementOne>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirementOne requirement)
        {
            if (context.User.HasClaim("IsEmployee", "Yes"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    public class RequirementTwo : IAuthorizationRequirement
    {
        public RequirementTwo(int minAge)
        {
            MinAge = minAge;
        }

        public int MinAge { get; set; }
    }

    public class Requirement2OneHandler : AuthorizationHandler<RequirementTwo>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirementTwo requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
