using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Viaggio
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string IdAutista { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Inizio { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? Fine { get; set; }
    public string Targa { get; set; }
    public int KmInizialiTarga { get; set; }
    public string TargaRimorchio { get; set; }
    public int KmInizialiRimorchio { get; set; }
    public List<GiornataLavorativa> Giornate { get; set; }
    public List<Rifornimento> Rifornimenti { get; set; }

    public Viaggio()
    {}

    public static List<Viaggio> getList(string idAutista = null, string targa = null, DateTime? dal = null, DateTime? al = null)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Viaggio>(DBClient.COLLECTION_VIAGGI);
        return collection.Find(x => (idAutista == null || x.IdAutista == idAutista)
        && (targa == null || x.Targa == targa || x.TargaRimorchio == targa)
        && (dal == null || (x.Inizio >= dal && x.Inizio <= al))).ToList();

    }

    public static Viaggio getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Viaggio>(DBClient.COLLECTION_VIAGGI);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Viaggio>(DBClient.COLLECTION_VIAGGI);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Viaggio>.Filter.Eq(x => x.Id, Id);
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
        var collection = DBClient.Instance.getDatabase().GetCollection<Viaggio>(DBClient.COLLECTION_VIAGGI);
        var f = Builders<Viaggio>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }


}