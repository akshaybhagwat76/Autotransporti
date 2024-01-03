using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class GiornataLavorativa
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Data { get; set; }
    public string Tipo { get; set; }
    public decimal Tariffa { get; set; }

    public GiornataLavorativa()
    {}
}