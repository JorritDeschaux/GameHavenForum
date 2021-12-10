using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
			UploadDate = DateTime.UtcNow;
		}

		[Required]
		public int PosterId { get; set; }

		[Required]
		[StringLength(int.MaxValue, MinimumLength = 1)]
		public string Title { get; set; }

		[Required]
		[StringLength(int.MaxValue, MinimumLength = 1)]
		public string Body { get; set; }

		public int? GameId { get; set; }

		[Required]
		public DateTime UploadDate { get; private set; }

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
				Comments = request.Comments,
				GameId = request.GameId,
				UploadDate = DateTime.UtcNow
			};

			await _context.Posts.AddAsync(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
