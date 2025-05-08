using BOMService.Application.BOMs.Common;
using MediatR;

namespace BOMService.Application.BOMs.Commands
{
    public class CreateBOMCommand : IRequest<Guid>
    {
        public CreateBOMRequest Payload { get; set; }

        public CreateBOMCommand(CreateBOMRequest payload)
        {
            Payload = payload;
        }
    }
}
