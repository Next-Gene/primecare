namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents the base class for all entities in the PrimeCare domain.
/// Provides a unique identifier property that is inherited by all derived entities.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }
}
