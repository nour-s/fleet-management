namespace Domain.Models;

public enum DeliveryPointType
{
    /// <summary>
    /// Only packages can be unloaded. Sacks and packages in sacks can not be unloaded.
    /// </summary>
    Branch = 1,
    /// <summary>
    /// Sacks, packages in sacks and packages that are not assigned to a sack can be unloaded.
    /// </summary>
    DistributionCentre,
    /// <summary>
    /// Only sacks and packages in sacks can be unloaded.
    /// </summary>
    TransferCentre
}

public enum SackState
{
    /// <summary>
    /// A sack take the state of created when first created and while packages are being loaded into it.
    /// </summary>
    Created = 1,
    Loaded = 3,
    Unloaded = 4
}

public enum PackageState
{
    /// <summary>
    /// Shipments take “created” state when they are first created
    /// </summary>
    Created = 1,

    /// <summary>
    /// When a shipment is loaded in a sack, the loaded shipment becomes “loaded in sack,”
    /// </summary>
    LoadedInSack,
    Loaded,
    Unloaded
}