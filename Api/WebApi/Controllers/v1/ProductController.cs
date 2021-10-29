using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Contacts.Commands;
using Application.Features.Contacts.Commands.CreateContact;
using Application.Features.Contacts.Commands.DeleteContactById;
using Application.Features.Contacts.Commands.UpdateContact;
using Application.Features.Contacts.Queries.GetAllContacts;
using Application.Features.Contacts.Queries.GetContactById;
using Application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ContactController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllContactsParameter filter)
        {
          
            return Ok(await Mediator.Send(new GetAllContactsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber  }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetContactByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateContactCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateContactCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteContactByIdCommand { Id = id }));
        }
    }
}
