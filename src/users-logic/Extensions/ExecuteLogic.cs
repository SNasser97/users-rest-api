namespace users_logic.Extensions
{
    using System;
    using System.Threading.Tasks;

    public static class ExecuteLogic
    {
        public static void ThrowException<TException>(Func<bool> func, string message = null)
            where TException : Exception, new()
        {
            if (func())
            {
                TException exception = Activator.CreateInstance(typeof(TException),
                    GetDefaultMessageFromCustomException(typeof(TException)) ?? message) as TException;
                throw exception;
            }
        }

        public static async Task ThrowExceptionAsync<TException>(Func<Task<bool>> func, string message = null)
            where TException : Exception, new()
        {
            if (await func())
            {
                TException exception = Activator.CreateInstance(typeof(TException),
                    GetDefaultMessageFromCustomException(typeof(TException)) ?? message) as TException;
                throw exception;
            }
        }

        // TODO: Refacotr, breaks OCP
        private static string GetDefaultMessageFromCustomException(Type ex)
        {
            switch (ex.Name)
            {
                case "CommandResponseException":
                    return "Response Id was empty";
                case "CommandRequestException":
                    return "Request Id was empty";
                case "QueryRequestException":
                    return "Request Id was empty";
                case "UserNotFoundException":
                    return "User not found";
                case "EmailExistsException":
                    return "Email already exists";
                case "InvalidAgeException":
                    return "Invalid date of birth";
                case "InvalidDateOfBirthException":
                    return "Ages 18 to 110 can only make a user!";
            }

            return null;
        }
    }
}