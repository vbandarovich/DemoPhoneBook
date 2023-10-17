using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhoneBookApp.Common.Enums;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.CQS.Commands;
using PhoneBookApp.Data.CQS.Queries;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Controllers
{
    [Route("api/phoneBook")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PhoneBookController> _logger;
        private readonly IMapper _mapper;

        public PhoneBookController(IMediator mediator, ILogger<PhoneBookController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                var result = await _mediator.Send(new GetAllContactsQuery());

                if (result.Any())
                {
                    _logger.LogInformation("Got all contacts from db.");

                    return Ok(result);
                }

                _logger.LogInformation("Contacts don't exist.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while getting all contacts with message: {ex.Message}");

                return StatusCode(500, "Error occurs while getting all contacts");
            }
        }

        [HttpGet("person")]
        public async Task<IActionResult> GetAllPersonContacts()
        {
            try
            {
                var result = await _mediator.Send(new GetAllPersonContactsQuery());

                if (result.Any())
                {
                    _logger.LogInformation("Got all person contacts from db.");

                    return Ok(result);
                }

                _logger.LogInformation("Person contacts don't exist.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while getting all person contacts with message: {ex.Message}");

                return StatusCode(500, "Error occurs while getting all person contacts");
            }
        }

        [HttpGet("publicOrganisation")]
        public async Task<IActionResult> GetAllPublicOrganisationContacts()
        {
            try
            {
                var result = await _mediator.Send(new GetAllPublicOrganisationContactsQuery());

                if (result.Any())
                {
                    _logger.LogInformation("Got all public organisation contacts from db.");

                    return Ok(result);
                }

                _logger.LogInformation("Public organisation contacts don't exist.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while getting all public organisation contacts with message: {ex.Message}");

                return StatusCode(500, "Error occurs while getting all public organisation contacts");
            }
        }

        [HttpGet("privateOrganisation")]
        public async Task<IActionResult> GetAllPrivateOrganisationContacts()
        {
            try
            {
                var result = await _mediator.Send(new GetAllPrivateOrganisationContactsQuery());

                if (result.Any())
                {
                    _logger.LogInformation("Got all private organisation contacts from db.");

                    return Ok(result);
                }

                _logger.LogInformation("Private organisation contacts don't exist.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while getting all private organisation contacts with message: {ex.Message}");

                return StatusCode(500, "Error occurs while getting all private organisation contacts");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddContact([FromBody] ContactModel model)
        {
            try
            {
                var result = await SendRequest(model);

                if (result is not null)
                {
                    _logger.LogInformation($"Contact added. Name: {result.Name}");

                    return Ok(result);
                }

                _logger.LogInformation($"Failed to add contact. Name: {result.Name}");

                return BadRequest("Failed while try to add contact. Please check model.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while adding new contact with message: {ex.Message}");

                return StatusCode(500, "Error occurs while adding new contact");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromBody] ContactModel model)
        {
            try
            {
                var result = await _mediator.Send(new UpdateContactCommand(model));

                if (result is not null)
                {
                    _logger.LogInformation($"Contact updated. Name: {result.Name}");

                    return Ok(result);
                }

                _logger.LogInformation($"Failed to update contact. Name: {result.Name}");

                return BadRequest("Failed while try to update contact. Please check model.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while updating contact with message: {ex.Message}");

                return StatusCode(500, "Error occurs while updating contact");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveContact(string id)
        {
            try
            {
                var contactId = Guid.Parse(id);

                await _mediator.Send(new RemoveContactCommand(contactId));

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs while removing contact with message: {ex.Message}");

                return StatusCode(500, "Error occurs while removing contact");
            }          
        }

        private async Task<PhoneBookEntity> SendRequest(ContactModel entity)
        {
            switch (entity.Type)
            {
                case ContactType.Person:
                    var personContact = _mapper.Map<PersonContactModel>(entity);
                    return await _mediator.Send(new AddPersonContactCommand(personContact));
                case ContactType.PublicOrganisation:
                    var publicOrganisationContact = _mapper.Map<PublicOrganisationContactModel>(entity);
                    return await _mediator.Send(new AddPublicOrganisationContactCommand(publicOrganisationContact));
                case ContactType.PrivateOrganisation:
                    var privateOrganisationContact = _mapper.Map<PrivateOrganisationContactModel>(entity);
                    return await _mediator.Send(new AddPrivateOrganisationContactCommand(privateOrganisationContact));
                default:
                    return null;
            }
        }
    }
}
