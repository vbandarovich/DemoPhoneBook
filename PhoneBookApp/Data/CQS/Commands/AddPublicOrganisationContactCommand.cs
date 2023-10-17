using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class AddPublicOrganisationContactCommand : IRequest<PhoneBookEntity>
    {
        public PublicOrganisationContactModel Contact { get; set; }

        public AddPublicOrganisationContactCommand(PublicOrganisationContactModel contact)
        {
            Contact = contact;
        }
    }

    public class AddPublicOrganisationContactCommandHandler : IRequestHandler<AddPublicOrganisationContactCommand, PhoneBookEntity>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AddPublicOrganisationContactCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddPublicOrganisationContactCommandHandler(AppDbContext db, ILogger<AddPublicOrganisationContactCommandHandler> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PhoneBookEntity> Handle(AddPublicOrganisationContactCommand request, CancellationToken cancellationToken)
        {
            if (!_db.PhoneBook.Any(x => x.Name.Equals(request.Contact.Name) && x.Type.Equals(request.Contact.Type)))
            {
                var entity = _mapper.Map<PhoneBookEntity>(request.Contact);

                await _db.PhoneBook.AddAsync(entity, cancellationToken);

                _logger.LogInformation($"Added new public organisation contact. Name: {request.Contact.Name}.");

                await _db.SaveChangesAsync(cancellationToken);
            }

            return await _db.PhoneBook.FirstAsync(x => x.Name.Equals(request.Contact.Name)
                && x.Type.Equals(request.Contact.Type), cancellationToken);
        }
    }
}
