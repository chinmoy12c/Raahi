using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class LocationGraph {

    Dictionary<int, KeyValuePair<int, Vector3>> nodeIds = new Dictionary<int, KeyValuePair<int, Vector3>>();
    Dictionary<int, int> idToNode = new Dictionary<int, int>();

    List<List<KeyValuePair<int, float>>> graph = new List<List<KeyValuePair<int, float>>>();
    Dictionary<string, Poi> pois = new Dictionary<string, Poi>();
    Dictionary<int, string> idToPoi = new Dictionary<int, string>();
    List<string> poiList = new List<string>();

    public LocationGraph(LocationRoute locationRoute) {
        foreach (Edge edge in locationRoute.edges) {
            From from = edge.from;
            To to = edge.to;
            if (!nodeIds.ContainsKey(from.id)) {
                nodeIds[from.id] = new KeyValuePair<int, Vector3>(graph.Count, convertToVector(from.vector));
                idToNode[graph.Count] = from.id;
                graph.Add(new List<KeyValuePair<int, float>>());
            }
            if (!nodeIds.ContainsKey(to.id)) {
                nodeIds[to.id] = new KeyValuePair<int, Vector3>(graph.Count, convertToVector(to.vector));
                idToNode[graph.Count] = to.id;
                graph.Add(new List<KeyValuePair<int, float>>());
            }
            float distance = Vector3.Distance(nodeIds[from.id].Value, nodeIds[to.id].Value);
            graph[nodeIds[from.id].Key].Add(new KeyValuePair<int, float>(nodeIds[to.id].Key, distance));
            graph[nodeIds[to.id].Key].Add(new KeyValuePair<int, float>(nodeIds[from.id].Key, distance));
        }

        foreach (Poi poi in locationRoute.pois) {
            pois[poi.poiName] = poi;
            poiList.Add(poi.poiName);
            idToPoi[poi.node.id] = poi.poiName;
        }
    }

    public Dictionary<int, KeyValuePair<int, Vector3>> getNodeIds() {
        return nodeIds;
    }

    public Dictionary<int, int> getIdToNode() {
        return idToNode;
    }

    public Dictionary<string, Poi> getPois() {
        return pois;
    }

    public Dictionary<int, string> getIdToPoi() {
        return idToPoi;
    }

    public List<string> getPoiList() {
        return poiList;
    }

    Vector3 convertToVector(string vectorStr) {
        var vectorData = vectorStr.Split('_');
        Vector3 vector = new Vector3(
            float.Parse(vectorData[0]),
            float.Parse(vectorData[1]),
            float.Parse(vectorData[2])
        );

        return vector;
    }

    public List<int> routeAllPaths(int currentNode) {
        currentNode = nodeIds[currentNode].Key;
        List<int> nodes = new List<int>();
        bool[] visited = new bool[graph.Count];
        dfs(currentNode, nodes, visited);
        List<int> pathNodes = new List<int>();
        foreach (int node in nodes)
            pathNodes.Add(idToNode[node]);
        return pathNodes;
    }

    void dfs(int currentNode, List<int> nodes, bool[] visited) {
        nodes.Add(currentNode);
        visited[currentNode] = true;
        foreach (KeyValuePair<int, float> adjNode in graph[currentNode]) {
            if (!visited[adjNode.Key]) {
                dfs(adjNode.Key, nodes, visited);
                nodes.Add(currentNode);
            }
        }
    }

    public List<int> routePath(int currentNode, int destinationNode) {
        currentNode = nodeIds[currentNode].Key;
        destinationNode = nodeIds[destinationNode].Key;
        int[] parent = new int[graph.Count];
        bool[] visited = new bool[graph.Count];
        parent[currentNode] = currentNode;
        findPath(currentNode, destinationNode, parent, visited);
        List<int> pathNodes = new List<int>();
        while (destinationNode != currentNode) {
            pathNodes.Add(idToNode[parent[destinationNode]]);
            destinationNode = parent[destinationNode];
        }
        pathNodes.Reverse();
        return pathNodes;
    }

    bool findPath(int currentNode, int destinationNode, int[] parent, bool[] visited) {
        visited[currentNode] = true;
        if (currentNode == destinationNode)
            return true;
        bool found = false;
        foreach (KeyValuePair<int, float> adjNode in graph[currentNode]) {
            if (!visited[adjNode.Key]) {
                parent[adjNode.Key] = currentNode;
                found = findPath(adjNode.Key, destinationNode, parent, visited);
                if (found) break;
            }
        }

        return found;
    }
}
