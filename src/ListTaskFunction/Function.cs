using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ListTaskFunction;

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
        var tasks = await dbContext.ScanAsync<FunctionLibrary.Task>(default).GetRemainingAsync();
        var resp = JsonSerializer.Serialize(tasks);
        return new APIGatewayProxyResponse
        {
            Body = resp,
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
