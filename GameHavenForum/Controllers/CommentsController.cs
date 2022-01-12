using Application.Features.Comments;
using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GameHavenForum.Controllers
{ 

    [Route("api/comments")]
    [ApiController]
    public class CommentsController : Controller
    {

		private IMediator _mediator;
		private ApplicationDbContext _context;

		public CommentsController(IMediator mediator, ApplicationDbContext context)
		{
			_mediator = mediator;
			_context = context;
		}

		[HttpPost("create")]
        public async Task<IActionResult> CreateComment([FromBody] Comment newComment)
        {
			var jwt = Request.Headers["Authorization"];

			int userId = await UserHelper.GetUserId(jwt);

			try
			{
				var result = await _mediator.Send(new CreateCommentCommand
				{
					Body = newComment.Body,
					UploadDate = newComment.UploadDate,
					CommentPosterId = userId,
					PostId = newComment.PostId
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
		public async Task<IActionResult> DeleteComment(int id)
		{
			var jwt = Request.Headers["Authorization"];

			int userId = await UserHelper.GetUserId(jwt);

			var result = await _mediator.Send(new DeleteCommentCommand { Id = id, CommentPosterId = userId });
			return result == true ? Ok(result) : BadRequest();
		}
	}
}
