namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using System.Threading.Tasks;
	using Grpc.Core;
	using Xunit;

	/// <summary>
	///   Helper functionality for tests.
	/// </summary>
	public static class TestHelper
	{
		/// <summary>
		///   The api key that is send to the server.
		/// </summary>
		public static string ApiKey => "63e3d887-4a74-4d68-864b-23fd267cc242";

		/// <summary>
		///   Checks the server responds with the expected status.
		/// </summary>
		/// <param name="clientCall">The async call to the server.</param>
		/// <param name="statusCode">The expected <see cref="Status.StatusCode" /> from the server.</param>
		/// <param name="detail">The error detail from <see cref="Status.Detail" />.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public static async Task CheckStatusCode(
			Func<Task> clientCall,
			StatusCode statusCode = StatusCode.Unauthenticated,
			string detail = null)
		{
			try
			{
				await clientCall();
				Assert.True(false, "Should have thrown RpcException");
			}
			catch (RpcException e)
			{
				Assert.Equal(statusCode, e.StatusCode);
				if (detail != null)
				{
					Assert.Equal(detail, e.Status.Detail);
				}
			}
			catch (Exception ex)
			{
				Assert.True(false, "Should have thrown RpcException: " + ex);
			}
		}
	}
}