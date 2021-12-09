using Application.Entities;
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

		[HttpGet("{id}")]
		public IActionResult GetPostById(int id)
		{

			var post = _mediator.Send(new GetPostCommand { Id = id });
			return post != null ? Ok(post) : NotFound();

		}

		[HttpPost("create")]
		public async Task<IActionResult> CreatePost([FromBody] Post newPost)
		{
			User user = new User();

			var jwt =  Request.Headers["Authorization"];

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://host.docker.internal:5000");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Authorization", jwt.ToString());

				var response = client.GetAsync("api/auth/verify");
				response.Wait();

				if (response == null) { return Unauthorized(); }

				var jsonString = await response.Result.Content.ReadAsStringAsync();
				user = JsonConvert.DeserializeObject<User>(jsonString);

				client.Dispose();
			}

			var result = _mediator.Send(new CreatePostCommand
			{
				Title = newPost.Title,
				Body = newPost.Body,
				PosterId = user.Id
			});

			return result != null ? Ok(result) : BadRequest();
		}

	}
}
