using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Posts
{
	public class DeletePostCommand : IRequest<bool>
	{
		public int Id { get; set; }

		public int PosterId { get; set; }
	}

	public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
	{

		private readonly ApplicationDbContext _context;

		public DeletePostCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
		{

			var post = await _context.Posts.Where(p => p.Id == request.Id && p.PosterId == request.PosterId).FirstOrDefaultAsync();

			if (post != null)
			{
				_context.Posts.Remove(post);
				await _context.SaveChangesAsync(cancellationToken);
				return true;
			}

			return false;
		}
	}
}
