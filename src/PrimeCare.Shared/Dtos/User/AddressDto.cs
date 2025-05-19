namespace PrimeCare.Shared.Dtos.User
{
    // This class is a Data Transfer Object (DTO) that holds user address information.
    public class AddressDto
    {
        // The first name of the user
        public string FirstName { get; set; }

        // The last name of the user
        public string LastName { get; set; }

        // The street address (e.g., house number and street name)
        public string Street { get; set; }

        // The city name where the user lives
        public string City { get; set; }

        // The state or region
        public string State { get; set; }

        // The postal or ZIP code
        public string ZipCode { get; set; }
    }
}
