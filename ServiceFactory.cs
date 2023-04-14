
using System;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;

public static class ServiceFactory
{
    public static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<AmazonDynamoDBClient>();
        services.AddValidatorsFromAssemblyContaining<OnConnectCommandValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OnConnectCommand).Assembly));
        return services.BuildServiceProvider();
    }
}