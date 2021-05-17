namespace users_logic.Extensions
{
    using System;
    using System.Threading.Tasks;

    public static class ExecuteLogic
    {
        public static void ThrowException<TException>(Func<bool> func)
            where TException : Exception, new()
        {
            if (func())
            {
                TException exception = Activator.CreateInstance(typeof(TException)) as TException;
                throw exception;
            }
        }

        /// <param name="message">Where message we can specify our own or the parameter name for argument null exceptions</param>
        public static void ThrowException<TException>(Func<bool> func, string message = null)
            where TException : Exception, new()
        {
            if (func())
            {
                TException exception = Activator.CreateInstance(typeof(TException), message) as TException;
                throw exception;
            }
        }

        public static async Task ThrowExceptionAsync<TException>(Func<Task<bool>> func)
            where TException : Exception, new()
        {
            if (await func())
            {
                TException exception = Activator.CreateInstance(typeof(TException)) as TException;
                throw exception;
            }
        }

        /// <param name="message">Where message we can specify our own or the parameter name for argument null exceptions</param>
        public static async Task ThrowExceptionAsync<TException>(Func<Task<bool>> func, string message = null)
            where TException : Exception, new()
        {
            if (await func())
            {
                TException exception = Activator.CreateInstance(typeof(TException), message) as TException;
                throw exception;
            }
        }
    }
}