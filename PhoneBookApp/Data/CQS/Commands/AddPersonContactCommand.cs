using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class AddPersonContactCommand : IRequest<PhoneBookEntity>
    {
        public PersonContactModel Contact { get; set; }

        public AddPersonContactCommand(PersonContactModel contact)
        {
            Contact = contact;
        }
    }

    public class AddContactCommandHandler : IRequestHandler<AddPersonContactCommand, PhoneBookEntity>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AddContactCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddContactCommandHandler(AppDbContext db, ILogger<AddContactCommandHandler> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PhoneBookEntity> Handle(AddPersonContactCommand request, CancellationToken cancellationToken)
        {
            if (!_db.PhoneBook.Any(x => x.Name.Equals(request.Contact.Name) && x.Type.Equals(request.Contact.Type)))
            {
                var entity = _mapper.Map<PhoneBookEntity>(request.Contact);

                await _db.PhoneBook.AddAsync(entity, cancellationToken);

                _logger.LogInformation($"Added new person contact. Name: {request.Contact.Name}.");

                await _db.SaveChangesAsync(cancellationToken);
            }

            return await _db.PhoneBook.FirstAsync(x => x.Name.Equals(request.Contact.Name) 
                && x.Type.Equals(request.Contact.Type), cancellationToken);
        }
    }
}
