using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
    [SerializeField]
    private Transform markerHolder;
    [SerializeField]
    private Dropdown poiSelector;

    private string BASE_URL = "http://13.235.48.103:8080";
    private const float NODE_ARROW_DISTANCE = 0.5f;
    private const float NODE_DRAW_DISTANCE = 0.7f;
    private LocationGraph locationGraph;
    private Color currentColor = Color.blue;

    void Start()
    {
        Camera.main.transform.rotation = Quaternion.identity;
        clearPaths();
        if (UnitySingleton.Instance.locationData != null) {
            Camera.main.transform.position = UnitySingleton.Instance.cameraPosition;
            LocationRoute locationRoute = JsonUtility.FromJson<LocationRoute>(UnitySingleton.Instance.locationData);
            locationGraph = new LocationGraph(locationRoute);
            poiSelector.ClearOptions();
            poiSelector.AddOptions(new List<string>(){"All Paths"});
            poiSelector.AddOptions(locationGraph.getPoiList());
            drawAllRoutes();
        }
        else {
            Debug.Log("No data");
        }
    }

    void Awake() {
        DontDestroyOnLoad(markerHolder);
    }

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
                poiSelector.ClearOptions();
                poiSelector.AddOptions(new List<string>(){"All Paths"});
                poiSelector.AddOptions(locationGraph.getPoiList());
            }
            else {
                Debug.Log("Failed: " + request.error);
            }
        }
    }

    public void drawAllRoutes() {
        clearPaths();
        int startNode = locationGraph.getIdToNode()[0];
        List<int> pathNodes = locationGraph.routeAllPaths(startNode);
        HashSet<int> rendered = new HashSet<int>();
        Vector3 from = new Vector3();
        for (int x = 0; x < pathNodes.Count; x++) {
            Debug.Log(pathNodes[x]);
            Vector3 nodeVector = locationGraph.getNodeIds()[pathNodes[x]].Value;
            if (!rendered.Contains(pathNodes[x])) {
                GameObject pathNode = locationGraph.getIdToPoi().ContainsKey(pathNodes[x]) ?
                    Instantiate(poiPointer, nodeVector, Quaternion.identity, markerHolder) :
                    Instantiate(turnPointer, nodeVector, Quaternion.identity, markerHolder);
                ((SignBehaviour)pathNode.GetComponentInChildren(typeof(SignBehaviour))).setNodeId(pathNodes[x]);
                if (x > 0) {
                    drawPath(from, nodeVector);            
                }
                rendered.Add(pathNodes[x]);
            }
            else {
                currentColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            from = locationGraph.getNodeIds()[pathNodes[x]].Value;
        }
    }

    public void drawRoute(string poi) {
        clearPaths();
        int startNode = getNearestNode();
        int endNode = locationGraph.getPois()[poi].node.id;
        List<int> pathNodes = locationGraph.routePath(startNode, endNode);
        pathNodes.Add(endNode);
        for (int x = 0; x < pathNodes.Count; x++) {
            Debug.Log(pathNodes[x]);
            Vector3 nodeVector = locationGraph.getNodeIds()[pathNodes[x]].Value;
            GameObject pathNode = locationGraph.getIdToPoi().ContainsKey(pathNodes[x]) ?
                Instantiate(poiPointer, nodeVector, Quaternion.identity, markerHolder) :
                Instantiate(turnPointer, nodeVector, Quaternion.identity, markerHolder);
            ((SignBehaviour)pathNode.GetComponentInChildren(typeof(SignBehaviour))).setNodeId(locationGraph.getNodeIds()[pathNodes[x]].Key);
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
            GameObject arrow = Instantiate(directionalPointer, nextDirectionalArrow, arrowRotation, markerHolder);
            arrow.GetComponentInChildren<Renderer>().material.color = currentColor;
            nextDirectionalArrow = nextDirectionalArrow + direction * NODE_DRAW_DISTANCE;
        }
    }

    int getNearestNode() {
        Vector3 from = Camera.main.transform.position;
        float minDistance = float.MaxValue;
        int nearestNode = -1;
        foreach (KeyValuePair<int, KeyValuePair<int, Vector3>> node in locationGraph.getNodeIds()) {
            float distance = Vector3.Distance(from, node.Value.Value);
            if (distance < minDistance) {
                minDistance = distance;
                nearestNode = node.Key;
            }
        }
        
        return nearestNode;
    }

    void clearPaths() {
        GameObject[] markerHolders = GameObject.FindGameObjectsWithTag("markerHolder");
        foreach (GameObject holder in markerHolders) {
            foreach (Transform marker in holder.transform) {
                Destroy(marker.gameObject);
            }
        }
    }
}
