using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Location {
    public int id;
    public string locationName;
}

[Serializable]
public class From {
    public int id;
    public string vector;
    public Location location;
}

[Serializable]
public class To {
    public int id;
    public string vector;
    public Location location;
}

[Serializable]
public class Edge {
    public int id;
    public From from;
    public To to;
}

[Serializable]
public class Node {
    public int id;
    public string vector;
    public Location location;
}

[Serializable]
public class Poi {
    public int id;
    public string poiName;
    public Node node;
}

[Serializable]
public class LocationRoute {
    public List<Edge> edges;
    public List<Poi> pois;
}

