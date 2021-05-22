namespace users_api.Extensions
{
    using System;
    using System.Threading.Tasks;
    using users_api.UserControllers.CommandControllers.Models.Response.Common;
    using users_logic.User.Logic.Command.Models.Response;

    public static class ControllerResponseModelExtensions
    {
        public static async Task<TControllerResponse> CaptureResponse<TControllerResponse, TCommandResponse>(Func<Task<TCommandResponse>> func)
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
    }
}