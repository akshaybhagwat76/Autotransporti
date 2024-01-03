using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Nota
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Data { get; set; }
    public string Testo { get; set; }
    public bool Pubblica { get; set; }

    public Nota()
    {}
}