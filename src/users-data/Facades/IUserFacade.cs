namespace users_data.Facades
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Models;
    using System;

    public interface IUserFacade
    {
        Task<bool> CanUserBeInsertedAsync(IEnumerable<UserRecord> records, IUserRecord record);

        Task<bool> IsUserRecordAgeValidAsync(int age);

        Task<bool> DoesUserRecordEmailExistAsync(IEnumerable<UserRecord> records, IUserRecord record);

        Task<int> GetUserAgeAsync(DateTime dob);
    }
}