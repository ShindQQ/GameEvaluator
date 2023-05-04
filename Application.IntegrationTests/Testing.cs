using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests;

public partial class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    public async Task RunBeforeAnyTestsAsync()
    {
        _factory = new CustomerApiFactory();
        await ((CustomerApiFactory)_factory).InitializeAsync();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator!.Send(request);
    }


}
