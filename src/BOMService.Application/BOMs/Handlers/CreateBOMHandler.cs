using AutoMapper;
using BOMService.Application.BOMs.Commands;
using BOMService.Domain.Entities;
using BOMService.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BOMService.Application.BOMs.Handlers
{
    public class CreateBOMHandler : IRequestHandler<CreateBOMCommand, Guid>
    {
        private readonly IBaseRepository<HouseModel> _houseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBOMHandler> _logger;

        public CreateBOMHandler(
            IBaseRepository<HouseModel> houseRepository,
            IMapper mapper,
            ILogger<CreateBOMHandler> logger)
        {
            _houseRepository = houseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateBOMCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateBOMHandler: {JsonSerializer.Serialize(request.Payload)}");

            var a = await _houseRepository.GetAllAsync();

            return Guid.NewGuid();
        }
    }
}
