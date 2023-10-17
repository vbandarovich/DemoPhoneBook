using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data.CQS.Queries
{
    public class GetAllContactsQuery : IRequest<List<PhoneBookEntity>>
    {
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllContactsQuery, List<PhoneBookEntity>>
    {
        private readonly AppDbContext _db;

        public GetAllProductsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PhoneBookEntity>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            return await _db.PhoneBook
                .ToListAsync(cancellationToken);
        }
    }
}
