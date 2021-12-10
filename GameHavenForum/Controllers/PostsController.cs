using Application.Features.Posts;
using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GameHavenForum.Controllers
{
	[Route("api/posts")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private IMediator _mediator;
		private ApplicationDbContext _context;

		public PostsController(IMediator mediator, ApplicationDbContext context)
		{
			_mediator = mediator;
			_context = context;
		}

		private async Task<int> GetUserId(string jwt)
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

		[HttpGet]
		public async Task<IActionResult> GetPosts()
		{

			var posts = await _mediator.Send(new GetAllPostsCommand { });
			return posts != null ? Ok(posts) : NotFound();

		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPostById(int id)
		{

			var post = await _mediator.Send(new GetPostCommand { Id = id });
			return post != null ? Ok(post) : NotFound();

		}

		[HttpPost("create")]
		public async Task<IActionResult> CreatePost([FromBody] Post newPost)
		{
			var jwt = Request.Headers["Authorization"];

			int userId = await GetUserId(jwt);

			try
			{
				var result = await _mediator.Send(new CreatePostCommand
				{
					Title = newPost.Title,
					Body = newPost.Body,
					PosterId = userId,
					GameId = newPost.GameId
				});

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
				throw;
			}	
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var jwt = Request.Headers["Authorization"];

			int userId = await GetUserId(jwt);

			var result = await _mediator.Send(new DeletePostCommand { Id = id, PosterId = userId});
			return result == true ? Ok(result) : BadRequest();
		}

	}
}
