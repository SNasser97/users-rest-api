namespace users_api.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_api.UserControllers.CommandControllers.Models.Response;
    using users_api.UserControllers.CommandControllers.Models.Response.Common;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_logic.User.Logic.Command.Models.Response;
    using users_logic.User.Logic.Query.Models.Response;

    public static class ControllerResponseModelExtensions
    {
        public static async Task<TControllerResponse> CaptureResponseAsync<TControllerResponse, TCommandResponse>(Func<Task<TCommandResponse>> func)
            where TControllerResponse : BaseUserControllerResponseModel, new()
            where TCommandResponse : BaseUserCommandResponseModel
        {
            try
            {
                TCommandResponse model = await func();
                return new TControllerResponse { Id = model.Id };
            }
            catch (Exception ex)
            {
                return new TControllerResponse { Error = ex.Message };
            }
        }

        public static async Task<DeleteUserControllerResponseModel> CaptureDeleteResponseAsync(Func<Task> func)
        {
            try
            {
                await func();
                return default(DeleteUserControllerResponseModel);
            }
            catch (Exception ex)
            {
                return new DeleteUserControllerResponseModel { Error = ex.Message };
            }
        }

        public static async Task<GetUserControllerResponseModel> CaptureGetResponseAsync(Func<Task<GetUserQueryResponseModel>> func)
        {
            try
            {
                return (await func()).ToControllerResponse();
            }
            catch (Exception ex)
            {
                return new GetUserControllerResponseModel { Error = ex.Message };
            }
        }

        public static async Task<IEnumerable<GetUserControllerResponseModel>> MapListToControllerResponsesAsync(this IEnumerable<GetUserQueryResponseModel> source)
            => await Task.WhenAll(source.Select(r => Task.Run(() => r.ToControllerResponse())));

        public static GetUserControllerResponseModel ToControllerResponse(this GetUserQueryResponseModel source)
        {
            return new GetUserControllerResponseModel
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = source.Age
            };
        }
    }
}