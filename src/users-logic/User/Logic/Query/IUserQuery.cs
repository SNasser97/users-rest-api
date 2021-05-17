namespace users_logic.User.Logic.Query
{
    using System.Threading.Tasks;

    public interface IUserQuery<TRequest, TResponse, TResponses>
    {
        Task<TResponse> GetReponseAsync(TRequest request);

        Task<TResponses> GetResponsesAsync();
    }
}