using BOMService.Application.BOMs.Commands;
using BOMService.Application.BOMs.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BOMService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateBOM")]
        public async Task<ActionResult> CreateBOM([FromBody] CreateBOMRequest request)
        {
            var command = new CreateBOMCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
