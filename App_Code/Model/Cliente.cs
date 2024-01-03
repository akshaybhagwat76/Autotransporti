using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Cliente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Descrizione { get; set; }
    public string RagioneSociale { get; set; }
    public List<DDT> DDT { get; set; }
    public Cliente()
    {}

    public static List<Cliente> getList()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Cliente>(DBClient.COLLECTION_CLIENTI);
        return collection.AsQueryable().ToList();

    }

    public static Cliente getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Cliente>(DBClient.COLLECTION_CLIENTI);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Cliente>(DBClient.COLLECTION_CLIENTI);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Cliente>.Filter.Eq(x => x.Id, Id);
            var result = collection.ReplaceOne(eq, this).IsAcknowledged;
            return result;
        }
        else
        {
            collection.InsertOne(this);
            return true;
        }
    }
    public static bool delete(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Cliente>(DBClient.COLLECTION_CLIENTI);
        var f = Builders<Cliente>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }


}