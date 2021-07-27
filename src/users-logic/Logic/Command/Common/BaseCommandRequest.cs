namespace users_logic.Logic.Command.Common
{
    using System;
    using System.Threading.Tasks;
    using users_data.Repositories;

    public abstract class BaseCommandRequest<TRequest, TResponse, TRecord> : ICommand<TRequest, TResponse>
    {
        protected readonly IWriteRepository<TRecord> writeRepository;
        protected readonly IReadRepository<TRecord> readRepository;

        public BaseCommandRequest(IWriteRepository<TRecord> writeRepository, IReadRepository<TRecord> readRepository)
        {
            this.readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
            this.writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        }

        public async Task<TResponse> ExecuteAsync(TRequest request)
        {
            return await this.OnExecuteAsync(request);
        }

        protected abstract Task<TResponse> OnExecuteAsync(TRequest request);
    }
}