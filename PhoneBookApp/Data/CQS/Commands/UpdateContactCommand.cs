using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class UpdateContactCommand : IRequest<PhoneBookEntity>
    {
        public ContactModel Contact { get; set; }

        public UpdateContactCommand(ContactModel contact)
        {
            Contact = contact;
        }
    }

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, PhoneBookEntity>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UpdateContactCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(AppDbContext db, ILogger<UpdateContactCommandHandler> logger, 
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PhoneBookEntity> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.PhoneBook.FirstOrDefaultAsync(x => 
                x.Name.Equals(request.Contact.Name) && x.Type.Equals(request.Contact.Type), cancellationToken: cancellationToken);

            if (entity is not null)
            {
                _mapper.Map(request.Contact, entity);

                await _db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Updated contact. Name: {entity.Name}");

                return entity;
            }

            _logger.LogInformation("Cannot update contact: it doesn't exist");

            return null;
        }
    }
}
