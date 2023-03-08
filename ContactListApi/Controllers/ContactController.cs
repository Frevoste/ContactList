using ContactListApi.Entities;
using ContactListApi.Models;
using ContactListApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Controllers
{
    [Route("api/contact")]
    [Authorize]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #region HttpGet
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Contact>> GetAll([FromQuery]ContactQuery query)
        {
            var contactsDtos = _contactService.GetAll(query);

            return Ok(contactsDtos);
        }

        //Ustalenie ścieżki do akcji id np. /api/contact/7 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Contact> Get([FromRoute] int id)
        {
            var contact = _contactService.GetById(id);
            return Ok(contact);
        }
        #endregion

        #region HttpPost
        [HttpPost]

        public ActionResult CreateContact([FromBody] CreateContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var id = _contactService.Create(dto);
                return Created($"/api/restaurant/{id}", null);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest("Email is not unique.");
                }
                return BadRequest();
            }

        }
        #endregion

        #region HttpDelete

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _contactService.Delete(id);
            return Ok();
        }
        #endregion

        #region HttpUpdate
        [HttpPatch("{id}")]
        public ActionResult Update([FromBody] UpdateContactDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _contactService.Update(id, dto);
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                return BadRequest();
            }
        }
        #endregion
    }
}
