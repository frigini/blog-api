using BlogApi.Application.Interfaces;
using BlogApi.Application.Responses;
using BlogApi.Web.Common;

namespace BlogApi.Web.Endpoints.Authentication;

public class AuthenticationEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/token", Handle)
            .WithName("GetToken")
            .WithOpenApi()
            .WithDescription("Get authentication token")
            .Produces<Response<string>>(StatusCodes.Status200OK);

    private static IResult Handle(ITokenService tokenService)
    {
        var token = tokenService.GenerateJwtToken();

        return TypedResults.Ok(new Response<string>(
            token,
            StatusCodes.Status200OK,
            "Token generated successfully"));
    }
}