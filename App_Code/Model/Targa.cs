using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Targa
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Codice { get; set; }
    public string TipoMezzo { get; set; }
    public int KmTotali { get; set; }
    public List<Asse> Assi { get; set; }
    public bool Attiva { get; set; }
    public Targa()
    {}

    public static List<Targa> getList(string codice = null, bool? attiva = null)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Targa>(DBClient.COLLECTION_TARGHE);
        return collection.Find(x => (codice == null || x.Codice == codice)
        && (attiva == null || x.Attiva == attiva)).ToList();

    }
    public static Targa getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Targa>(DBClient.COLLECTION_TARGHE);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }
    public static bool isCodeUsed(string code)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Targa>(DBClient.COLLECTION_TARGHE);
        return collection.Find(x => x.Codice == code).Any();
    }

    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Targa>(DBClient.COLLECTION_TARGHE);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Targa>.Filter.Eq(x => x.Id, Id);
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
        var collection = DBClient.Instance.getDatabase().GetCollection<Targa>(DBClient.COLLECTION_TARGHE);
        var f = Builders<Targa>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }

    public void updateTotalKM(int addingKm)
    {
        KmTotali += addingKm;
        if (Assi != null)
            foreach (var ax in Assi)
                if (ax.Pneumatici != null)
                    foreach (var p in ax.Pneumatici)
                        if (p.InFunzione)
                            p.KmTotali += addingKm;
        insertOrUpdate();
    }

}