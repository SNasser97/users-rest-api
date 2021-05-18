namespace users_api.UserControllers.CommandControllers.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Request.Common;

    public class CreateUserControllerRequestModel
    {
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        public BaseUserCommandRequest ToCommandRequest()
        {
            return new CreateUserCommandRequest
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                DateOfBirth = DateTime.ParseExact(this.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }
    }
}