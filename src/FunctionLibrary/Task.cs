using Amazon.DynamoDBv2.DataModel;

namespace FunctionLibrary;

[DynamoDBTable("tasks")]
public class Task
{
    [DynamoDBHashKey("id")]
    public Guid Id { get; set; }
    [DynamoDBProperty("description")]
    public string? Description { get; set; }
    [DynamoDBProperty("title")]
    public string? Title { get; set; }
}