using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EncurtaURL.Entities;
using EncurtaURL.Models;
using EncurtaURL.Persistence;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EncurtaURL.Controllers
{
    [ApiController]
    [Route("api/shortenedLinks")]
    public class ShortenedLinksController : ControllerBase
    {
        private readonly EncurtaUrlDbContext _context;

        public ShortenedLinksController(EncurtaUrlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Log.Information("A listagem foi chamada!");

            return Ok(_context.Links);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id == id);

            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }
        
        /// <summary>
        /// Cadastrar um link encurtado
        /// </summary>
        /// <remarks>
        /// {"title": "meu-github", "destinationLink": "https://github.com/pablorangell"}
        /// </remarks>
        /// <param name="model">Dados de link</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Add(AddOrUpdateShortenedLinkModel model)
        {
            var domain = HttpContext.Request.Host.Value;

            var link = new ShortenedCustomLink(model.Title, model.DestinationLink, domain);

            _context.Links.Add(link);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = link.Id}, link);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, AddOrUpdateShortenedLinkModel model)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id == id);

            if (link == null)
            {
                return NotFound();
            }

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id == id);

            if (link == null)
            {
                return NotFound();
            }

            _context.Links.Remove(link);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("/{code}")]
        public IActionResult RedirectLink(string code)
        {
            var link = _context.Links.SingleOrDefault(x => x.Code == code);

            if (link == null)
            {
                return NotFound();
            }

            return Redirect(link.DestinationLink);
        }
    }
}