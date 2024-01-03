using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class APILogs
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime Timestamp { get; set; }
    public string EventType { get; set; }
    public string Controller { get; set; }
    public string Method{ get; set; }
    public string Message { get; set; }

    public APILogs()
    {
        Timestamp = DateTime.Now;
    }

    public static bool insert(APILogs logs)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<APILogs>(DBClient.COLLECTION_API_LOG);
        collection.InsertOne(logs);
        return true;
    }
}