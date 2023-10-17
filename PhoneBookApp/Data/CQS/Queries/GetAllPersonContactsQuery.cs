using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Common.Enums;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Queries
{
    public class GetAllPersonContactsQuery : IRequest<List<PhoneBookEntity>>
    {
    }

    public class GetAllPersonContactsQueryHandler : IRequestHandler<GetAllPersonContactsQuery, List<PhoneBookEntity>>
    {
        private readonly AppDbContext _db;

        public GetAllPersonContactsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PhoneBookEntity>> Handle(GetAllPersonContactsQuery request, CancellationToken cancellationToken)
        {
            return await _db.PhoneBook
                .Where(x => x.Type.Equals(ContactType.Person))
                .ToListAsync(cancellationToken);
        }
    }
}
