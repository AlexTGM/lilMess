namespace lilMess.Client.Models
{
    using System.ComponentModel.DataAnnotations;

    using lilMess.Mvvm;
    using lilMess.Tools;

    public class LoginModel : ValidatableModel
    {
        private string serverInfo = "127.0.0.1:9997";

        private string userName = "User";

        private Address address = new Address { Ip = "127.0.0.1", Port = 9997 };

        [Required]
        [CustomValidation(typeof(LoginModel), "ValidEndPoint")]
        public string ServerInfo
        {
            get { return this.serverInfo; }
            set { this.Set("ServerInfo", ref this.serverInfo, value); }
        }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName
        {
            get { return this.userName; }
            set { this.Set("UserName", ref this.userName, value); }
        }

        public Address Address
        {
            get { return this.address; }
        }

        public static ValidationResult ValidEndPoint(object obj, ValidationContext context)
        {
            return AddressParser.IsValidEndPoint((string)obj, ref ((LoginModel)context.ObjectInstance).address)
                       ? ValidationResult.Success
                       : new ValidationResult("Введенный адрес некорректен!", new[] { "ServerInfo" });
        }
    }
}