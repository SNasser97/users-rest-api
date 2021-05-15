namespace users_test
{
    using System;
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
    }
}
