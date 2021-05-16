namespace users_logic.User.Facades
{
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using users_data.Entities;

    public interface IUserLogicFacade
    {
        /// <summary>
        /// To check an existing email from a collection of user records
        /// </summary>
        /// <param name="records">The user records that contain user properties</param>
        /// <param name="email">The updated request email to be checked</param>
        Task<bool> DoesUserEmailAlreadyExistAsync(IEnumerable<BaseUserRecordWithId> userId, string email);

        Task<bool> IsAgeValidAsync(int age);

        /// <param name="dateOfBirth">The requested dateOfBirth to be checked</param>
        Task<int> GetCalculatedUsersAgeAsync(DateTime dateOfBirth);
    }
}