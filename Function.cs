using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Flyingdarts.Lambdas.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var services = ServiceFactory.GetServiceProvider();
var innerHandler = new InnerHandler(services.GetRequiredService<IMediator>());
var serializer = new DefaultLambdaJsonSerializer(x => x.PropertyNameCaseInsensitive = true);

// The function handler that will be called for each Lambda event
var handler = async (APIGatewayProxyRequest request) =>
{
    var socketRequest = request.To<OnConnectCommand>(serializer);
    return await innerHandler.Handle(socketRequest);
};

await LambdaBootstrapBuilder.Create(handler, serializer)
    .Build()
    .RunAsync();

