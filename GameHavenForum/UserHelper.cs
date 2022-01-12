using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GameHavenForum
{
    public static class UserHelper
    {
		public static async Task<int> GetUserId(string jwt)
		{
			int userId;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://host.docker.internal:5000");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Authorization", jwt.ToString());

				var response = client.GetAsync("api/auth/userIdByToken");
				response.Wait();

				if (response.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized) { return 0; }

				var jsonString = await response.Result.Content.ReadAsStringAsync();
				userId = JsonConvert.DeserializeObject<int>(jsonString);

				client.Dispose();
			}

			return userId;
		}

	}
}
