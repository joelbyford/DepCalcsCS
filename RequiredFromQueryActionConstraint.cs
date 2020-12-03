using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepCalcsCS
{

    //Class to enforce required HTTP querystring attributes or return a 404 when they are not present.
    public class RequiredFromQueryActionConstraint : IActionConstraint
    {
        private readonly string _parameter;

        //Required override
        public RequiredFromQueryActionConstraint(string parameter)
        {
            _parameter = parameter;
        }

        //Required override
        public int Order => 999;

        //Required override
        public bool Accept(ActionConstraintContext context)
        {
            if (!context.RouteContext.HttpContext.Request.Query.ContainsKey(_parameter))
            {
                return false;
            }

            return true;
        }
    }
}
