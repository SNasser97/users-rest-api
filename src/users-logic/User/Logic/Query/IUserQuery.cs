namespace users_logic.User.Logic.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserQuery<TRequest, TResponse, TResponses>
    {
        Task<TResponse> GetReponseAsync(TRequest request);

        Task<TResponses> GetReponsesAsync();
    }
}