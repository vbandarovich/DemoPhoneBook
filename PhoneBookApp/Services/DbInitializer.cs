using MediatR;
using PhoneBookApp.Common.Interfaces;
using PhoneBookApp.Data.CQS.Commands;

namespace PhoneBookApp.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IMediator _mediator;

        public DbInitializer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InitializeAsync()
        {
            await _mediator.Send(new InitPhoneBookCommand());
        }
    }
}
