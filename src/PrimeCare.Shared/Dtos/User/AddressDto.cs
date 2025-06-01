namespace PrimeCare.Shared.Dtos.User
{
    public class AddressDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; } // Added email property

    }
}
