using Amazon.Lambda.APIGatewayEvents;
using MediatR;

public class OnConnectCommand : IRequest<APIGatewayProxyResponse>
{
    public string PlayerId { get; set; } = null;
    public string ConnectionId { get; set; } = null;

}