using System;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

public sealed class DBClient
{
    public static string COLLECTION_OPERATORI = "operatori";
    public static string COLLECTION_AUTISTI = "autisti";
    public static string COLLECTION_TARGHE = "targhe";
    public static string COLLECTION_MANUTENZIONI = "manutenzioni";
    public static string COLLECTION_CLIENTI = "clienti";
    public static string COLLECTION_VIAGGI = "viaggi";
    public static string COLLECTION_API_LOG = "api_logs";

    private static readonly Lazy<DBClient> lazy = new Lazy<DBClient>(() => new DBClient());
    public static DBClient Instance
    {
        get { return lazy.Value; }
    }
    private MongoClient client;
    private IMongoDatabase db;
    private DBClient()
    {
        client = new MongoClient(ConfigurationManager.AppSettings["DbConnectionString"]);
        db = client.GetDatabase(ConfigurationManager.AppSettings["DbName"]);
    }

    public IMongoDatabase getDatabase()
    {
        if (db == null)
            db = client.GetDatabase(ConfigurationManager.AppSettings["DbName"]);

        return db;
    }
}
