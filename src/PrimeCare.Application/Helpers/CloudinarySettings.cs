namespace PrimeCare.Application.Helpers;

/// <summary>
/// Represents the settings required to configure Cloudinary integration.
/// </summary>
public class CloudinarySettings
{
    /// <summary>
    /// Gets or sets the Cloudinary cloud name.
    /// </summary>
    public string? CloudName { get; set; }

    /// <summary>
    /// Gets or sets the Cloudinary API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the Cloudinary API secret.
    /// </summary>
    public string? ApiSecret { get; set; }
}
