using Microsoft.AspNet.Http.Security;
using Microsoft.AspNet.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OAuthBearerClient.Controllers
{
	[Authorize]
    public class TodoListController : Controller
    {
		private const string todoListBaseAddress = "http://localhost:62089/";
		private const string accessToken = "";
		private string todoListResourceId = "https://TusharTest.onmicrosoft.com/TodoListService-ManualJwt";
		private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
		private static string clientId = "ba7651c2-53c2-442a-97c2-3d60ea42f403";
		private static string appKey = "ZpmbkS5HXK3Zpl5rC8nO/0vDTW7TZI0qjqDdJgKqihk=";
		private static string authority = "https://login.windows.net/tushartest.onmicrosoft.com";

		// GET: /<controller>/
		public async Task<ActionResult> Index()
        {
			Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = null;

			try
			{

				//string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
				AuthenticationContext authContext = new AuthenticationContext(authority);
				ClientCredential credential = new ClientCredential(clientId, appKey);
//#if ASPNET50
//				result = await authContext.AcquireTokenAsync(todoListResourceId, clientId, new Uri("http://localhost:42023"), null);
//#endif
				result = await authContext.AcquireTokenAsync(todoListResourceId, credential);

				//
				// Retrieve the user's To Do List.
				//
				HttpClient client = new HttpClient();
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, todoListBaseAddress + "/api/todolist");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
				HttpResponseMessage response = await client.SendAsync(request);

				//
				// Return the To Do List in the view.
				//
				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					ViewBag.Message = "Success";
					ViewBag.Content = responseContent;
					return View();
				}
				else
				{
					ViewBag.Message = "Failure";

					//
					// If the call failed with access denied, then drop the current access token from the cache, 
					//     and show the user an error indicating they might need to sign-in again.
					//
					if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						var todoTokens = authContext.TokenCache.ReadItems().Where(a => a.Resource == todoListResourceId);
						foreach (TokenCacheItem tci in todoTokens)
							authContext.TokenCache.DeleteItem(tci);

						ViewBag.ErrorMessage = "UnexpectedError";
						return View();
					}
				}
			}
			catch (Exception ee)
			{
				ViewBag.Message = ee.Message + ee.InnerException;
				//ViewBag.Message = "AuthorizationRequired";
				return View();
			}


			//
			// If the call failed for any other reason, show the user an error.
			//
			return View("Error");


		}

	}
}
