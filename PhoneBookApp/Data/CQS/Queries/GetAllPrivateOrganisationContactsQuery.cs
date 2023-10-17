using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Enums;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Queries
{
    public class GetAllPrivateOrganisationContactsQuery : IRequest<List<PhoneBookEntity>>
    {
    }

    public class GetAllPrivateOrganisationContactsQueryHandler : IRequestHandler<GetAllPrivateOrganisationContactsQuery, List<PhoneBookEntity>>
    {
        private readonly AppDbContext _db;

        public GetAllPrivateOrganisationContactsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PhoneBookEntity>> Handle(GetAllPrivateOrganisationContactsQuery request, CancellationToken cancellationToken)
        {
            return await _db.PhoneBook
                .Where(x => x.Type.Equals(ContactType.PrivateOrganisation))
                .ToListAsync(cancellationToken);
        }
    }
}
