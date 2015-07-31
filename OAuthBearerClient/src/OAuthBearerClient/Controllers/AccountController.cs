using System;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;

namespace OAuthBearerClient.Controllers
{
	public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = null)
        {
			if (Context.User == null || !Context.User.Identity.IsAuthenticated)
			{
				Context.Authentication.ChallengeAsync(OpenIdConnectAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

        public IActionResult LogOff()
        {
			Context.Authentication.SignOutAsync(OpenIdConnectAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

    }
}