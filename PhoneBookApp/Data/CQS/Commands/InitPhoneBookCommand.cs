using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using PhoneBookApp.Common.Constants;
using PhoneBookApp.Common.Enums;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Commands
{
    public class InitPhoneBookCommand : IRequest
    {
    }

    public class InitPhoneBookCommandHandler : IRequestHandler<InitPhoneBookCommand>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<InitPhoneBookCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InitPhoneBookCommandHandler(AppDbContext db, ILogger<InitPhoneBookCommandHandler> logger,
            IMediator mediator, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Handle(InitPhoneBookCommand request, CancellationToken cancellationToken)
        {
            await _db.Database.EnsureCreatedAsync(cancellationToken);

            if (!_db.PhoneBook.Any())
            {
                try
                {
                    using var reader = new StreamReader(FileNameConstants.PhoneBookFileName);

                    var json = reader.ReadToEnd();

                    var phoneBook = JsonConvert.DeserializeObject<List<PhoneBookEntity>>(json);

                    if (phoneBook is not null)
                    {
                        foreach (var contact in phoneBook)
                        {
                            await SendRequest(contact, cancellationToken);
                        }
                    }

                    _logger.LogInformation("Cannot init db: contacts are null.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Init database failed. Message: {ex.Message}");
                }
            }
        }

        private async Task SendRequest(PhoneBookEntity entity, CancellationToken cancellationToken)
        {
            switch (entity.Type)
            {
                case ContactType.Person:
                    var personContact = _mapper.Map<PersonContactModel>(entity);
                    await _mediator.Send(new AddPersonContactCommand(personContact), cancellationToken);
                    break;
                case ContactType.PublicOrganisation:
                    var publicOrganisationContact = _mapper.Map<PublicOrganisationContactModel>(entity);
                    await _mediator.Send(new AddPublicOrganisationContactCommand(publicOrganisationContact), cancellationToken);
                    break;
                case ContactType.PrivateOrganisation:
                    var privateOrganisationContact = _mapper.Map<PrivateOrganisationContactModel>(entity);
                    await _mediator.Send(new AddPrivateOrganisationContactCommand(privateOrganisationContact), cancellationToken);
                    break;
                default:
                    break;
            }
        }
    }
}
