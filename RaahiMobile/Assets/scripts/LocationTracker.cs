using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LocationTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject turnPointer;
    [SerializeField]
    private GameObject poiPointer;
    [SerializeField]
    private GameObject directionalPointer;

    private string BASE_URL = "http://10.21.85.102:8080";
    private const float NODE_ARROW_DISTANCE = 0.5f;
    private const float NODE_DRAW_DISTANCE = 0.7f;
    private LocationGraph locationGraph;

    public static UnityWebRequest PostWebRequest(String uri, string body) {
        return PostWebRequest(new Uri(uri), body, Encoding.UTF8);
    }
 
    public static UnityWebRequest PostWebRequest(Uri uri, string body, Encoding encoding) {
        byte[] bodyData = encoding.GetBytes(body);
        return new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST, new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
    }

    public void fetchLocation(string locationName) {
        Route route = new Route();
        route.setLocationName(locationName);
        StartCoroutine(getLocationData(route));
    }

    IEnumerator getLocationData(Route route) {
        string jsonData = JsonUtility.ToJson(route);
        Debug.Log(jsonData);
        using (UnityWebRequest request = PostWebRequest(BASE_URL + "/getLocation", jsonData)) {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                Debug.Log("Response: " + request.downloadHandler.text);
                LocationRoute locationRoute = JsonUtility.FromJson<LocationRoute>(request.downloadHandler.text);
                locationGraph = new LocationGraph(locationRoute);
            }
            else {
                Debug.Log("Failed: " + request.error);
            }
        }
    }

    public void drawAllRoutes() {
        Debug.Log(locationGraph.getIdToNode());
        int startNode = locationGraph.getIdToNode()[0];
        Debug.Log(startNode);
        List<int> pathNodes = locationGraph.routeAllPaths(startNode);
        HashSet<int> rendered = new HashSet<int>();
        Vector3 from = new Vector3();
        for (int x = 0; x < pathNodes.Count; x++) {
            Debug.Log(pathNodes[x]);
            Vector3 nodeVector = locationGraph.getNodeIds()[pathNodes[x]].Value;
            if (!rendered.Contains(pathNodes[x])) {
                GameObject pathNode = locationGraph.getIdToPoi().ContainsKey(pathNodes[x]) ?
                    Instantiate(poiPointer, nodeVector, Quaternion.identity) :
                    Instantiate(turnPointer, nodeVector, Quaternion.identity);
                if (x > 0) {
                    drawPath(from, nodeVector);            
                }
                rendered.Add(pathNodes[x]);
            }
            from = locationGraph.getNodeIds()[pathNodes[x]].Value;
        }
    }

    public void drawRoute(int startNode, int endNode) {
        List<int> pathNodes = locationGraph.routePath(startNode, endNode);
        pathNodes.Add(endNode);
        for (int x = 0; x < pathNodes.Count; x++) {
            Debug.Log(pathNodes[x]);
            Vector3 nodeVector = locationGraph.getNodeIds()[pathNodes[x]].Value;
            GameObject pathNode = locationGraph.getIdToPoi().ContainsKey(pathNodes[x]) ?
                Instantiate(poiPointer, nodeVector, Quaternion.identity) :
                Instantiate(turnPointer, nodeVector, Quaternion.identity);
            if (x > 0) {
                Vector3 from = locationGraph.getNodeIds()[pathNodes[x-1]].Value;
                drawPath(from, nodeVector);            
            }
        }
    }

    void drawPath(Vector3 from, Vector3 to) {
        Vector3 direction = (to - from).normalized;
        float nodesDistance = Vector3.Distance(from, to);
        Vector3 nextDirectionalArrow = from + direction * NODE_DRAW_DISTANCE;
        while (Vector3.Distance(from, nextDirectionalArrow) + NODE_ARROW_DISTANCE <= nodesDistance) {
            Quaternion arrowRotation = new Quaternion();
            arrowRotation.SetFromToRotation(
                new Vector3(0,0,1),
                direction
            );
            Instantiate(directionalPointer, nextDirectionalArrow, arrowRotation);
            nextDirectionalArrow = nextDirectionalArrow + direction * NODE_DRAW_DISTANCE;
        }
    }
}
