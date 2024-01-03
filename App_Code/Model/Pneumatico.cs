using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utility;

public class Pneumatico
{
    public string Marca { get; set; }
    public int KmTotali { get; set; }
    public int Posizione { get; set; }
    public bool InFunzione { get; set; }

    public Pneumatico()
    {}
}