using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Enums;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Queries
{
    public class GetAllPublicOrganisationContactsQuery : IRequest<List<PhoneBookEntity>>
    {
    }

    public class GetAllPublicOrganisationContactsQueryHandler : IRequestHandler<GetAllPublicOrganisationContactsQuery, List<PhoneBookEntity>>
    {
        private readonly AppDbContext _db;

        public GetAllPublicOrganisationContactsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PhoneBookEntity>> Handle(GetAllPublicOrganisationContactsQuery request, CancellationToken cancellationToken)
        {
            return await _db.PhoneBook
                .Where(x => x.Type.Equals(ContactType.PublicOrganisation))
                .ToListAsync(cancellationToken);
        }
    }
}
