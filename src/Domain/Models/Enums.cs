namespace Domain.Models;

public enum DeliveryPointType
{
    Branch = 1,
    DistributionCentre,
    TransferCentre
}

public enum SackState
{
    Created = 1,
    Loaded = 3,
    Unloaded = 4
}

public enum PackageState
{
    Created = 1,
    LoadedInSack,
    Loaded,
    Unloaded
}