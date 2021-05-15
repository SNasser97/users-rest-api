namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Extensions;
    using users_data.Facades;
    using users_data.Models;

    public class InMemoryUserWriteRepository : IWriteRepository
    {
        private readonly IDictionary<Guid, UserRecord> users;
        private readonly IUserFacade userFacade;

        // mocking usage
        public InMemoryUserWriteRepository() : this(
            new Dictionary<Guid, UserRecord>(),
            new UserFacade())
        {
        }

        public InMemoryUserWriteRepository(
            IDictionary<Guid, UserRecord> users,
            IUserFacade userFacade)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.userFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
        }

        public async Task<Guid> CreateAsync(CreateUserRecord record)
        {
            if (!await this.userFacade.CanUserBeInsertedAsync(this.users.Values, record))
            {
                return await Task.FromResult(Guid.Empty);
            }

            int age = await this.userFacade.GetUserAgeAsync(record.DateOfBirth);

            UserRecord userRecord = record.ToUserRecord(age);

            this.users.Add(userRecord.Id, userRecord);

            return await Task.FromResult(userRecord.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.FromResult(this.users.Remove(id));
        }

        public async Task<Guid> UpdateAsync(UpdateUserRecord record)
        {
            bool isValidRecord = await this.userFacade.CanUserBeInsertedAsync(this.users.Values, record);
            int age = await this.userFacade.GetUserAgeAsync(record.DateOfBirth);

            if (!isValidRecord)
            {
                return await Task.FromResult(Guid.Empty);
            }

            if (this.users.TryGetValue(record.Id, out UserRecord found))
            {
                found.FirstName = record.FirstName;
                found.LastName = record.LastName;
                found.Email = record.Email;
                found.DateOfBirth = record.DateOfBirth;
                found.Age = age;
            };

            return await Task.FromResult(found.Id);
        }
    }
}
