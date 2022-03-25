using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LocationScanner : MonoBehaviour {

    [SerializeField]
    private GameObject poiPointer;
    [SerializeField]
    private GameObject directionalPointer;
    [SerializeField]
    private GameObject turnPointer;

    private const float POI_DRAW_DISTANCE = 1.5f;
    private const float NODE_DRAW_DISTANCE = 0.7f;
    private const float NODE_ARROW_DISTANCE = 0.5f;
    private string BASE_URL = "http://10.21.85.102:8080";

    private Camera mainCamera;
    private List<GameObject> pathNodes;

	void Start () {
		mainCamera = Camera.main;
        pathNodes = new List<GameObject>();
	}

    public static UnityWebRequest PostWebRequest(String uri, string body) {
        return PostWebRequest(new Uri(uri), body, Encoding.UTF8);
    }
 
    public static UnityWebRequest PostWebRequest(Uri uri, string body, Encoding encoding) {
        byte[] bodyData = encoding.GetBytes(body);
        return new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST, new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
    }

	public void placeTurn() {
        Vector3 poiLocation = mainCamera.transform.position + mainCamera.transform.forward * POI_DRAW_DISTANCE;
        GameObject pathNode = Instantiate(turnPointer, poiLocation, Quaternion.identity);
        pathNodes.Add(pathNode);
        if (pathNodes.Count > 1) {
            Vector3 from = pathNodes[pathNodes.Count - 2].transform.position;
            Vector3 to = pathNodes[pathNodes.Count - 1].transform.position;
            drawPath(from, to);            
        }
    }

    public void placePoi(string locationName, string poiName) {
        Vector3 poiLocation = mainCamera.transform.position + mainCamera.transform.forward * POI_DRAW_DISTANCE;
        GameObject pathNode = Instantiate(poiPointer, poiLocation, Quaternion.identity);
        pathNodes.Add(pathNode);
        if (pathNodes.Count > 1) {
            Vector3 from = pathNodes[pathNodes.Count - 2].transform.position;
            Vector3 to = pathNodes[pathNodes.Count - 1].transform.position;
            drawPath(from, to);            
        }

        initiateSavePath(locationName, poiName);
    }

    private void drawPath(Vector3 from, Vector3 to) {
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

    void initiateSavePath(string locationName, string poiName) {
        Route route = new Route();
        route.setLocationName(locationName);
        route.setPoiName(poiName);
        List<string> nodes = new List<string>();
        foreach (GameObject node in pathNodes) {
            nodes.Add(route.convertVector(node.transform.position));
        }
        route.setNodes(nodes);
        StartCoroutine(postRouteData(route));
    }

    IEnumerator postRouteData(Route route) {
        string jsonData = JsonUtility.ToJson(route);
        Debug.Log(jsonData);
        using (UnityWebRequest request = PostWebRequest(BASE_URL + "/saveRoute", jsonData)) {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                Debug.Log("Success");
            }
            else {
                Debug.Log("Failed");
            }
        }
    }
}