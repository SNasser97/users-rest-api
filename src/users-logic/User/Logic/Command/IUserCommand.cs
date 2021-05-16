namespace users_logic.User.Logic.Command
{
    using System.Threading.Tasks;

    public interface IUserCommand<TRequest, T2Request, TResponse>
    {
        Task<TResponse> CreateUserAsync(TRequest request);

        Task<TResponse> UpdateUserAsync(T2Request request);
    }
}