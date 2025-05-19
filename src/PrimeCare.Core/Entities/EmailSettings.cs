namespace PrimeCare.Core.Entities
{
    // This class represents configuration settings for sending emails
    public class EmailSettings
    {
        // SMTP server address (e.g., smtp.gmail.com)
        public string SmtpHost { get; set; }

        // Port used for SMTP (e.g., 587 for TLS or 465 for SSL)
        public int SmtpPort { get; set; }

        // Username or email address used to authenticate with the SMTP server
        public string SmtpUser { get; set; }

        // Password for the SMTP user
        public string SmtpPass { get; set; }
    }
}
