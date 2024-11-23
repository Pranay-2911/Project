using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private  readonly IDocumentService _documentService;

        public DocumentController(IDocumentService service)
        {
            _documentService = service;
        }

        [HttpPost]
        public IActionResult Add(Document document)
        {
            var newId = _documentService.Add(document);
            return Ok(newId);
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if(_documentService.Delete(id))
                return Ok(id);
            return BadRequest();
        }
    }
}
