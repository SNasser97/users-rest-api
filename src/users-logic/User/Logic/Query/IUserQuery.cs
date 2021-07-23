namespace users_logic.User.Logic.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserQuery<TRequest, TResponse>
    {
        Task<TResponse> GetResponseAsync(TRequest request);

        Task<IEnumerable<TResponse>> GetResponsesAsync();
    }
}