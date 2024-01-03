using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Autista
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Telefono { get; set; }
    public bool Abilitato { get; set; }
    public List<Tariffa> Tariffe { get; set; }
    public List<Nota> Note { get; set; }

    public Autista()
    {}

    public static List<Autista> getList()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        return collection.AsQueryable().ToList();

    }
    public static bool isUsernameUsed(string username)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        return collection.Find(x => x.Username == username).Any();

    }
    public static Autista getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public static Autista login(string username, string password)
    {
        string cryptoPassword = Crypto.Encrypt(password, true);
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        return collection.Find(x => x.Username == username && x.Password == cryptoPassword).FirstOrDefault();
    }
    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Autista>.Filter.Eq(x => x.Id, Id);
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
        var collection = DBClient.Instance.getDatabase().GetCollection<Autista>(DBClient.COLLECTION_AUTISTI);
        var f = Builders<Autista>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }


}