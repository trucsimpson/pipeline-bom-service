﻿namespace BOMService.Domain.Entities
{
    public class ProductToBuildingPhaseModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int BuildingPhaseId { get; set; }
    }
}
