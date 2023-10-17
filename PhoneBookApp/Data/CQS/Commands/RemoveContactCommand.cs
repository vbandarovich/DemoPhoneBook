using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class RemoveContactCommand : IRequest
    {
        public Guid Id { get; set; }

        public RemoveContactCommand(Guid id)
        {
            Id = id;
        }
    }

    public class RemoveContactCommandHandler : IRequestHandler<RemoveContactCommand>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<RemoveContactCommandHandler> _logger;

        public RemoveContactCommandHandler(AppDbContext db, ILogger<RemoveContactCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Handle(RemoveContactCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.PhoneBook.FirstOrDefaultAsync(x => 
                x.Id.Equals(request.Id), cancellationToken: cancellationToken);

            _db.PhoneBook.Remove(entity);

            _logger.LogInformation($"Removed contact with Id: {entity.Id}");

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
