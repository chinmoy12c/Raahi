using System;
using UnityEngine;
using System.Collections.Generic;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Location {
    public int id { get; set; }
    public string locationName { get; set; }
}

public class From {
    public int id { get; set; }
    public string vector { get; set; }
    public Location location { get; set; }
}

public class To {
    public int id { get; set; }
    public string vector { get; set; }
    public Location location { get; set; }
}

public class Edge {
    public int id { get; set; }
    public From from { get; set; }
    public To to { get; set; }
}

public class Node {
    public int id { get; set; }
    public string vector { get; set; }
    public Location location { get; set; }
}

public class Pois {
    public int id { get; set; }
    public string poiName { get; set; }
    public Node node { get; set; }
}

[Serializable]
public class LocationRouteResponse {
    public List<Edge> edges { get; set; }
    public List<Pois> pois { get; set; }
}

