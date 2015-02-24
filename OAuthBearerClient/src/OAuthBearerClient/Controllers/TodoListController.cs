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

#if ASPNET50
using System.Diagnostics.Tracing;
using OAuthBearerClient.Logging;
#endif

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

#if ASPNET50
		SampleListener listener = new SampleListener();
#endif

		// GET: /<controller>/
		public async Task<ActionResult> Index()
        {
			//HttpClient httpClient = new HttpClient();

			//string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImtyaU1QZG1Cdng2OHNrVDgtbVBBQjNCc2VlQSJ9.eyJhdWQiOiJodHRwczovL1R1c2hhclRlc3Qub25taWNyb3NvZnQuY29tL1RvZG9MaXN0U2VydmljZS1NYW51YWxKd3QiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9hZmJlY2UwMy1hZWFhLTRmM2YtODVlNy1jZTA4ZGQyMGNlNTAvIiwiaWF0IjoxNDE4MzMwNjE0LCJuYmYiOjE0MTgzMzA2MTQsImV4cCI6MTQxODMzNDUxNCwidmVyIjoiMS4wIiwidGlkIjoiYWZiZWNlMDMtYWVhYS00ZjNmLTg1ZTctY2UwOGRkMjBjZTUwIiwiYW1yIjpbInB3ZCJdLCJvaWQiOiI1Mzk3OTdjMi00MDE5LTQ2NTktOWRiNS03MmM0Yzc3NzhhMzMiLCJ1cG4iOiJWaWN0b3JAVHVzaGFyVGVzdC5vbm1pY3Jvc29mdC5jb20iLCJ1bmlxdWVfbmFtZSI6IlZpY3RvckBUdXNoYXJUZXN0Lm9ubWljcm9zb2Z0LmNvbSIsInN1YiI6IkQyMm9aMW9VTzEzTUFiQXZrdnFyd2REVE80WXZJdjlzMV9GNWlVOVUwYnciLCJmYW1pbHlfbmFtZSI6Ikd1cHRhIiwiZ2l2ZW5fbmFtZSI6IlZpY3RvciIsImFwcGlkIjoiNjEzYjVhZjgtZjJjMy00MWI2LWExZGMtNDE2Yzk3ODAzMGI3IiwiYXBwaWRhY3IiOiIwIiwic2NwIjoidXNlcl9pbXBlcnNvbmF0aW9uIiwiYWNyIjoiMSJ9.N_Kw1EhoVGrHbE6hOcm7ERdZ7paBQiNdObvp2c6T6n5CE8p0fZqmUd-ya_EqwElcD6SiKSiP7gj0gpNUnOJcBl_H2X8GseaeeMxBrZdsnDL8qecc6_ygHruwlPltnLTdka67s1Ow4fDSHaqhVTEk6lzGmNEcbNAyb0CxQxU6o7Fh0yHRiWoLsT8yqYk8nKzsHXfZBNby4aRo3_hXaa4i0SZLYfDGGYPdttG4vT_u54QGGd4Wzbonv2gjDlllOVGOwoJS6kfl1h8mk0qxdiIaT_ChbDWgkWvTB7bTvBE-EgHgV0XmAo0WtJeSxgjsG3KhhEPsONmqrSjhIUV4IVnF2w";

			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
			//string path = Context.Request.Path.Value;
			//HttpResponseMessage response = await httpClient.GetAsync(todoListAddress);

			//if (response.IsSuccessStatusCode)
			//{
			//	string responseContent = await response.Content.ReadAsStringAsync();
			//	ViewBag.Message = "Success";
			//	ViewBag.Content = responseContent;
			//}
			//else
			//{
			//	ViewBag.Message = "Failure";
			//}


			Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = null;

			try
			{

#if ASPNET50

				listener.EnableEvents(AdalOption.AdalEventSource, EventLevel.Verbose);
#endif
				//string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
				AuthenticationContext authContext = new AuthenticationContext(authority);
				ClientCredential credential = new ClientCredential(clientId, appKey);
//#if ASPNET50
//				result = await authContext.AcquireTokenAsync(todoListResourceId, clientId, new Uri("http://localhost:42023"), null);
//#endif
				result = await authContext.AcquireTokenAsync(todoListResourceId, credential);
#if ASPNET50

				if (!String.IsNullOrEmpty(listener.TraceBuffer))
				{
					Console.WriteLine(listener.TraceBuffer);
				}
#endif

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
