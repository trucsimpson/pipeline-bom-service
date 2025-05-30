﻿namespace BOMService.Domain.Enums
{
    public enum TraceNodeTypes
    {
        None = -1,
        GetProductsToX = 1,
        DoProductFlipping = 2,
        DoProductAssembliesOnetimespecsetProductconversion = 5,
        DoProductAssembliesOnetimespecsetStyleconversion = 6,
        DoProductAssembliesOptiontospecsetProductconversion = 7,
        DoProductAssembliesOptiontospecsetStyleconversion = 8,
        DoProductAssembliesSubcomponentrolloutParentswap = 9,
        DoProductAssembliesSubcomponentrolloutNewsub = 10,
        DoProductAssembliesFinaloutputproduct = 19,
        DoProductAssemblies = 11,
        MergeLikeRows = 12,
        ApplyWasteRoundingWasted = 13,
        ApplyWasteRoundingRounded = 14,
        RemoveZeroQuantities = 15,
        AddParentProducts = 16,
        AFinalBomProduct = 17,
        WriteBomDataToDatabase = 18,
        JobConfigBomFinalDeltaProduct = 20,
        JobConfigBomMergedOptionProduct = 21,
        JobConfigBomBuildingPhaseViewProduct = 22,
        JobConfigBomBuildingPhaseViewPhase = 23,
        LeftBehindProduct = 24,
        JobProductFlipping = 25,
        PerformBidMasterConversion = 26,
        DoProductAssembliesSubcomponentrolloutParentswapAssumeParent = 27,
        DoProductAssembliesSubcomponentrolloutParentswapStaticAll = 28,
        DoProductAssembliesSubcomponentrolloutParentswapPhaseRule = 29,
        DoProductAssembliesSubcomponentrolloutNewsubAssumeParent = 30,
        DoProductAssembliesSubcomponentrolloutNewsubStaticAll = 31,
        DoProductAssembliesSubcomponentrolloutNewsubPhaseRule = 32,
        PreProductAssemblyBOMLogicRule_ChangeProduct = 33,
        DuringProductAssemblyBOMLogicRule_ChangeProduct = 34,
        PostProductAssemblyBOMLogicRule_ChangeProduct = 35,
        PreProductAssemblyBOMLogicRule_AddProduct = 36,
        DuringProductAssemblyBOMLogicRule_AddProduct = 37,
        PostProductAssemblyBOMLogicRule_AddProduct = 38,
        PreProductAssemblyBOMLogicRule_RemoveProduct = 39,
        DuringProductAssemblyBOMLogicRule_RemoveProduct = 40,
        PostProductAssemblyBOMLogicRule_RemoveProduct = 41,
        EndPreProductAssemblyLoop = 42,
        EndDuringProductAssemblyLoop = 43,
        HouseOrientationFlip = 44,
        PostBomProductAdjustmentBomLogicRuleChangeProduct = 45
    }
}
