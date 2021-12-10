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

	public class GetAllPostsCommand : IRequest<List<Post>>
	{
		
	}

	public class GetAllPostsCommandHandler : IRequestHandler<GetAllPostsCommand, List<Post>>
	{

		private readonly ApplicationDbContext _context;

		public GetAllPostsCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Post>> Handle(GetAllPostsCommand request, CancellationToken cancellationToken)
		{
			var posts = _context.Posts.ToList();

			foreach (var post in posts)
			{
				List<Comment> comments = _context.Comments.Where(c => c.PostId == post.Id).OrderBy(c => c.UploadDate).ToList();
				post.Comments = comments;
			}

			return posts;
			
		}
	}
}
