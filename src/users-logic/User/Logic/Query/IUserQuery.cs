namespace users_logic.User.Logic.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserQuery<TRequest, TResponse>
    {
        Task<TResponse> GetReponseAsync(TRequest request);

        Task<IEnumerable<TResponse>> GetReponsesAsync();
    }
}