using System.Collections.Generic;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Flyingdarts.Lambdas.Shared;
using MediatR;

public class OnConnectCommandHandler : IRequestHandler<OnConnectCommand, APIGatewayProxyResponse>
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;
    public OnConnectCommandHandler(IAmazonDynamoDB dynamoDb, string tableName)
    {
        _dynamoDb = dynamoDb;
        _tableName = tableName;
    }
    public async Task<APIGatewayProxyResponse> Handle(OnConnectCommand request, CancellationToken cancellationToken)
    {

        if (request != null)
            await CreateSignallingRecord(request.ConnectionId);

        if (!string.IsNullOrEmpty(request!.PlayerId))
            await UpdateSignallingRecord(request.ConnectionId, request.PlayerId);

        return Responses.Created(JsonSerializer.Serialize(request));
    }
    private async Task CreateSignallingRecord(string connectionId)
    {
        var ddbRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { "ConnectionId", new AttributeValue{ S = connectionId }}
            }
        };

        await _dynamoDb.PutItemAsync(ddbRequest);
    }
    private async Task UpdateSignallingRecord(string connectionId, string playerId)
    {
        var ddbRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { "ConnectionId", new AttributeValue{ S = connectionId }},
                { "PlayerId", new AttributeValue{ S = playerId }}
            }
        };

        await _dynamoDb.PutItemAsync(ddbRequest);
    }
}