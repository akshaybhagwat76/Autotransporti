using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class DDT
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Data { get; set; }
    public string File { get; set; }
    public string IdAutista { get; set; }

    public DDT()
    {}
}