namespace users_api.UserControllers.Common
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseUserControllerRequestModel
    {
        [FromBody]
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [FromBody]
        [StringLength(128)]
        public string LastName { get; set; }

        [FromBody]
        [Required]
        public string DateOfBirth { get; set; }

        [FromBody]
        [Required]
        public string Email { get; set; }
    }
}