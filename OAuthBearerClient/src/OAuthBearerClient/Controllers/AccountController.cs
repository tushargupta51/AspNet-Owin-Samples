using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Security;
using Microsoft.AspNet.Security.OpenIdConnect;

namespace OAuthBearerClient.Controllers
{
	public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = null)
        {
			if (Context.User == null || !Context.User.Identity.IsAuthenticated)
			{
				Context.Response.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

        public IActionResult LogOff()
        {
			Context.Response.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType);
			return RedirectToAction("Index", "Home");
		}

    }
}