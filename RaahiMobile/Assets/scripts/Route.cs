using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Route {
    public List<String> nodes;
    public string locationName;
    public string poiName;

    public string convertVector(Vector3 loc) {
        return loc.x + "_" + loc.y + "_" + loc.z;
    }

    public void setNodes(List<String> nodes) {
        this.nodes = nodes;
    }

    public void setLocationName(string locationName) {
        this.locationName = locationName;
    }

    public void setPoiName(string poiName) {
        this.poiName = poiName;
    }

    public List<String> getNodes() {
        return nodes;
    }

    public string getLocation() {
        return locationName;
    }

    public string getPoiName() {
        return poiName;
    }
}