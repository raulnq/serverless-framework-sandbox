using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PostTaskFunction;

public class Function
{
    public record RegisterTaskRequest(string Description, string Title);
    public record RegisterTaskResponse(Guid Id);
    private readonly AmazonDynamoDBClient client;

    private readonly DynamoDBContext dbContext;
    public Function()
    {
        client = new AmazonDynamoDBClient();
        dbContext = new DynamoDBContext(client);
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        var req = JsonSerializer.Deserialize<RegisterTaskRequest>(input.Body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;
        var task = new FunctionLibrary.Task() { Id = Guid.NewGuid(), Description = req.Description, Title = req.Title };
        await dbContext.SaveAsync(task);
        var resp = JsonSerializer.Serialize(new RegisterTaskResponse(task.Id));
        return new APIGatewayProxyResponse
        {
            Body = resp,
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
