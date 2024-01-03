using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Manutenzione
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Descrizione { get; set; }
    public int KmDa { get; set; }
    public int KmA { get; set; }
    public Manutenzione()
    {}

    public static List<Manutenzione> getList(int? km = null)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Manutenzione>(DBClient.COLLECTION_MANUTENZIONI);
        return collection.Find(x => km == null || (x.KmDa <= km.Value && x.KmA >= km.Value))
            .ToList().OrderBy(x => x.KmDa).ToList();

    }

    public static Manutenzione getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Manutenzione>(DBClient.COLLECTION_MANUTENZIONI);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Manutenzione>(DBClient.COLLECTION_MANUTENZIONI);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Manutenzione>.Filter.Eq(x => x.Id, Id);
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
        var collection = DBClient.Instance.getDatabase().GetCollection<Manutenzione>(DBClient.COLLECTION_MANUTENZIONI);
        var f = Builders<Manutenzione>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }


}