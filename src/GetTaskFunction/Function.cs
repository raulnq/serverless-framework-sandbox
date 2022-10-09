using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetTaskFunction;

public class Function
{
    private readonly AmazonDynamoDBClient client;
    private readonly DynamoDBContext dbContext;
    public Function()
    {
        client = new AmazonDynamoDBClient();
        dbContext = new DynamoDBContext(client);
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        var id = input.PathParameters["id"];
        var task = await dbContext.LoadAsync<FunctionLibrary.Task>(new Guid(id));
        if (task == null)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 404
            };
        }

        var resp = JsonSerializer.Serialize(task);
        return new APIGatewayProxyResponse
        {
            Body = resp,
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
