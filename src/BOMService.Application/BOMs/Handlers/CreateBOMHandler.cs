using AutoMapper;
using BOMService.Application.BOMs.Commands;
using BOMService.Application.Common.Interfaces;
using BOMService.Application.DTOs;
using BOMService.Domain.Entities;
using BOMService.Domain.Enums;
using BOMService.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace BOMService.Application.BOMs.Handlers
{
    public class CreateBOMHandler : IRequestHandler<CreateBOMCommand, Guid>
    {
        private readonly IBOMEngineService _BOMEngineService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBOMHandler> _logger;

        public CreateBOMHandler(
            IBOMEngineService BOMEngineService,
            IMapper mapper,
            ILogger<CreateBOMHandler> logger)
        {
            _BOMEngineService = BOMEngineService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateBOMCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateBOMHandler: {JsonSerializer.Serialize(request.Payload)}");

            await _BOMEngineService.RunAsync();

            return Guid.NewGuid();
        }
    }
}
