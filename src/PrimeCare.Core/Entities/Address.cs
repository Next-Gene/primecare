namespace PrimeCare.Core.Entities
{
    public class Address
    {

        public int Id { get; set; }
        public string

            FirstName
        { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;

        public string AppUserId { get; set; }

        public ApplicationUser AppUser { get; set; }

    }
}