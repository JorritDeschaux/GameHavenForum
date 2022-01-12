using Application.Persistence.Infrastructure;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Comments
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public int CommentPosterId { get; set; }
    }

    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
		private readonly ApplicationDbContext _context;

		public DeleteCommentCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
		{

			var comment = await _context.Comments.Where(c => c.Id == request.Id && c.CommentPosterId == request.CommentPosterId).FirstOrDefaultAsync();

			if (comment != null)
			{
				_context.Comments.Remove(comment);
				await _context.SaveChangesAsync(cancellationToken);
				return true;
			}

			return false;
		}
	}
}
