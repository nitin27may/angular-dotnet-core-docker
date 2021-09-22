using System;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Repository;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        [HttpGet]
        //[CustomAuthorization]
        public async Task<IActionResult> Get()
        {
            var userDetails = HttpContext.Items["User"];
            
            var result = await _contactRepository.FindAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _contactRepository.FindByID(Convert.ToInt32(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            var result = await _contactRepository.Add(contact);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contact contact)
        {
            var contactEntity = await _contactRepository.FindByID(id);
            if (contactEntity == null)
            {
                return NotFound();
            }
            var result = await _contactRepository.Update(contact);
            return Ok(result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Contact> contactDetails)
        {
            Contact contactEntity = await _contactRepository.FindByID(id);
            if (contactEntity == null)
            {
                return NotFound();
            }

            contactDetails.ApplyTo(contactEntity, ModelState);
            var result = await _contactRepository.Update(contactEntity);
            return Ok(result);
        }
    }
}
