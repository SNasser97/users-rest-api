namespace users_test
{
    using System;
    using System.Threading.Tasks;
    using Xunit.Sdk;

    public static class Exceptions<TException>
        where TException : Exception
    {
        public static void Handle(Action action, Action<TException> error)
        {
            try
            {
                action();
                throw new XunitException("Should not continue");
            }
            catch (TException ex)
            {
                error(ex);
            }
        }

        public static async Task HandleAsync(Func<Task> funcAsync, Action<TException> error)
        {
            try
            {
                await funcAsync();
                throw new XunitException("Should not continue");
            }
            catch (TException ex)
            {
                error(ex);
            }
        }

        public static async Task HandleAsync(Func<Task> func)
        {
            try
            {
                await func();
                // should continue..
            }
            catch (Exception ex)
            {
                throw new XunitException($"Should not throw: {ex.Message}");
            }
        }
    }
}
