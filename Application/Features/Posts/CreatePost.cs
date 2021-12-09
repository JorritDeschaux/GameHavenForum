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

	public class CreatePostCommand : IRequest<int>
	{
		public CreatePostCommand()
		{
			Comments = new List<Comment>();
			Pictures = new List<Picture>();
			UploadDate = DateTime.UtcNow;
		}

		public int PosterId { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public DateTime UploadDate { get; private set; }

		public List<Picture> Pictures { get; set; }

		public List<Comment> Comments { get; set; }

	}

	public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
	{

		private readonly ApplicationDbContext _context;

		public CreatePostCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
		{

			var entity = new Post
			{
				PosterId = request.PosterId,
				Title = request.Title,
				Body = request.Body,
				Pictures = request.Pictures,
				Comments = request.Comments,
			};

			_context.Posts.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
