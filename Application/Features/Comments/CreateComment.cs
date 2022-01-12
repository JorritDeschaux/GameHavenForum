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

namespace Application.Features.Comments
{
    public class CreateCommentCommand : IRequest<int>
    {
        public CreateCommentCommand()
        {
            UploadDate = DateTime.UtcNow;
        }


        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Body { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int CommentPosterId { get; set; }

    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateCommentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Comment
            {
                Body = request.Body,
                PostId = request.PostId,
                UploadDate = request.UploadDate,
                CommentPosterId = request.CommentPosterId
            };

            await _context.Comments.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

    }
}
