using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class AddPrivateOrganisationContactCommand : IRequest<PhoneBookEntity>
    {
        public PrivateOrganisationContactModel Contact { get; set; }

        public AddPrivateOrganisationContactCommand(PrivateOrganisationContactModel contact)
        {
            Contact = contact;
        }
    }

    public class AddPrivateOrganisationContactCommandHandler : IRequestHandler<AddPrivateOrganisationContactCommand, PhoneBookEntity>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AddPrivateOrganisationContactCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddPrivateOrganisationContactCommandHandler(AppDbContext db, ILogger<AddPrivateOrganisationContactCommandHandler> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PhoneBookEntity> Handle(AddPrivateOrganisationContactCommand request, CancellationToken cancellationToken)
        {
            if (!_db.PhoneBook.Any(x => x.Name.Equals(request.Contact.Name) && x.Type.Equals(request.Contact.Type)))
            {
                var entity = _mapper.Map<PhoneBookEntity>(request.Contact);

                await _db.PhoneBook.AddAsync(entity, cancellationToken);

                _logger.LogInformation($"Added new private organisation contact. Name: {request.Contact.Name}.");

                await _db.SaveChangesAsync(cancellationToken);
            }

            return await _db.PhoneBook.FirstAsync(x => x.Name.Equals(request.Contact.Name)
                && x.Type.Equals(request.Contact.Type), cancellationToken);
        }
    }
}
