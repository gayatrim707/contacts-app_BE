using contacts_app_BE.DAL;
using contacts_app_BE.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace contacts_app_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactServices _contactServices;

        public ContactsController(ContactServices contactServices)
        {
            _contactServices = contactServices;
        }
        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAllContact()
        {
            var contacts = await _contactServices.GetAllContact();
            return Ok(contacts);
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetById(int id)
        {

            var contacts = await _contactServices.GetContactById(id);
            if (contacts == null)
            {
                return NotFound();
            }
            return Ok(contacts);
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
                await _contactServices.AddContactAsync(contact);
                return Ok(CreatedAtAction(nameof(GetAllContact), new { id = contact.Id }, contact));
            
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
        {
            var result = await _contactServices.UpdateContactAsync(id, updatedContact);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var result = await _contactServices.DeleteContactAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
