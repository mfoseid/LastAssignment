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
using System.Dynamic;
using System.Net.Http;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TheLastAssignment
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

        public static readonly HttpClient apiClient = new HttpClient();
        private static AmazonDynamoDBClient databaseClient = new AmazonDynamoDBClient();
        private static string tableName = "FoxPics";

        public async Task<RandomFox> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string url = "https://randomfox.ca/floof/";
            string apiString = await apiClient.GetStringAsync(url);
            RandomFox myFox = JsonConvert.DeserializeObject<RandomFox>(apiString);
            Table foxes = Table.LoadTable(databaseClient, tableName);

            PutItemOperationConfig config = new PutItemOperationConfig();
            config.ReturnValues = ReturnValues.AllOldAttributes;

            await foxes.PutItemAsync(Document.FromJson(JsonConvert.SerializeObject(myFox)), config);
            return myFox;
        }
    }
}
