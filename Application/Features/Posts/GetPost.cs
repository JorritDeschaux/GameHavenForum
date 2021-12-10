using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Posts
{

	public class GetPostCommand : IRequest<Post>
	{
		public int Id { get; set; }
	}

	public class GetPostCommandHandler : IRequestHandler<GetPostCommand, Post>
	{

		private readonly ApplicationDbContext _context;

		public GetPostCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Post> Handle(GetPostCommand request, CancellationToken cancellationToken)
		{
			var post = await _context.Posts.FindAsync(request.Id);

			List<Comment> comments = _context.Comments.Where(c => c.PostId == post.Id).OrderBy(c => c.UploadDate).ToList();
			post.Comments = comments;

			return post != null ? post : null;
		}
	}
}
