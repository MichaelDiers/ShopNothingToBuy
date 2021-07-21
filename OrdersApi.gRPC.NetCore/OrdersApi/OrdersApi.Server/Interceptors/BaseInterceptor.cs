namespace OrdersApi.Server.Interceptors
{
	using System;
	using Grpc.Core;
	using Grpc.Core.Interceptors;

	public class BaseInterceptor : Interceptor
	{
		/// <summary>
		///   Check if the given guid is valid add it to the <see cref="ServerCallContext.UserState" />.
		/// </summary>
		/// <param name="context">
		///   A valid guid is added to <see cref="ServerCallContext.UserState" /> with key
		///   <paramref name="userStateName" />.
		/// </param>
		/// <param name="guid">The guid that is validated.</param>
		/// <param name="userStateName">The key used in <see cref="ServerCallContext.UserState" /> for the valid guid.</param>
		/// <exception cref="RpcException">Is thrown if the guid is not valid.</exception>
		protected static void ValidateGuidAndSetToContext(ServerCallContext context, string guid, string userStateName)
		{
			if (string.IsNullOrWhiteSpace(guid) || !Guid.TryParse(guid, out var parsedGuid) || parsedGuid == Guid.Empty)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, userStateName));
			}

			context.UserState.Add(userStateName, parsedGuid);
		}
	}
}