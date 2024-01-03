using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Operatore
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public Operatore()
    {}

    public static List<Operatore> getList(bool includeSuperadmin = false)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Operatore>(DBClient.COLLECTION_OPERATORI);
        if (!includeSuperadmin)
            return collection.Find(x => x.Role != UserRole.Superadmin).ToList();
        else
            return collection.AsQueryable().ToList();
    }

    public static Operatore getDetail(string id)
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Operatore>(DBClient.COLLECTION_OPERATORI);
        return collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public static Operatore login(string username, string password)
    {
        string cryptoPassword = Crypto.Encrypt(password, true);
        var collection = DBClient.Instance.getDatabase().GetCollection<Operatore>(DBClient.COLLECTION_OPERATORI);
        return collection.Find(x => x.Username == username && x.Password == cryptoPassword).FirstOrDefault();
    }
    public bool insertOrUpdate()
    {
        var collection = DBClient.Instance.getDatabase().GetCollection<Operatore>(DBClient.COLLECTION_OPERATORI);
        if (!string.IsNullOrEmpty(Id))
        {
            var eq = Builders<Operatore>.Filter.Eq(x => x.Id, Id);
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
        var collection = DBClient.Instance.getDatabase().GetCollection<Operatore>(DBClient.COLLECTION_OPERATORI);
        var f = Builders<Operatore>.Filter.Eq(x => x.Id, id);
        var r = collection.DeleteOne(f);
        return r.IsAcknowledged;
    }


}