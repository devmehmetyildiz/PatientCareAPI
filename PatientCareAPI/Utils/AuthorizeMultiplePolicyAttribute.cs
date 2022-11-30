using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Utils
{
    public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
    {
        public AuthorizeMultiplePolicyAttribute(string roles) : base(typeof(AuthorizeMultiplePolicyFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizeMultiplePolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string _roles { get; private set; }

        public AuthorizeMultiplePolicyFilter(string roles, IAuthorizationService authorization)
        {
            _roles = roles;
            _authorization = authorization;

        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(UserAuthory.Admin))
            {
                return;
            }
            foreach (var role in _roles.Split(','))
            {
                var roleAuthorized = context.HttpContext.User.IsInRole(role);
                if (!roleAuthorized)
                {
                    context.Result = new ObjectResult(new ResponseModel { Status = "Role Error", Massage = "Kullanıcının " + role + " yetkisi bulunmamaktadır." }) { StatusCode = 403 };
                    return;
                }
            }
        }
    }
}
