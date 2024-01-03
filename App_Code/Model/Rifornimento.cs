using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Rifornimento
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Data { get; set; }
    public int Km { get; set; }
    public decimal LtCarburante { get; set; }
    public decimal? CostoCarburante { get; set; }

    public Rifornimento()
    {}
}