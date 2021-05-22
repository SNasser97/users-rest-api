namespace users_logic.User.Logic.Query
{
    using System.Threading.Tasks;

    public interface IUserQuery<TRequest, TResponse, TResponses>
    {
        Task<TResponse> GetResponseAsync(TRequest request);

        Task<TResponses> GetResponsesAsync();
    }
}