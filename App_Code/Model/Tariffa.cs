using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Tariffa
{
    public string Tipo { get; set; }
    public decimal Valore { get; set; }

    public Tariffa()
    {}
}