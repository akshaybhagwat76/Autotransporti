using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Asse
{
    public List<Pneumatico> Pneumatici { get; set; }

    public Asse()
    {}
}