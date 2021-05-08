using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LastAssignmentLoad
{
    public class RandomFox
    {
        public RandomFox(string v1, string v2)
        {
            image = v1;
            link = v2;
        }

        public string image { get; set; }
        public string link { get; set; }
    }

    public class Function
    {
        private static AmazonDynamoDBClient databaseClient = new AmazonDynamoDBClient();
        private static string tableName = "FoxPics";

        public async Task<RandomFox> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string foxId = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("image", out foxId);
            GetItemResponse res = await databaseClient.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
                {
                    { "image", new AttributeValue { S = foxId } }
                }
            );


            Document myDoc = Document.FromAttributeMap(res.Item);
            RandomFox myFox = JsonConvert.DeserializeObject<RandomFox>(myDoc.ToJson());
            return myFox;
        }
    }
}
