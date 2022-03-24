using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LocationTracker : MonoBehaviour
{
    private string BASE_URL = "http://192.168.20.31:8080";

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
                LocationRouteResponse locationRoute = JsonUtility.FromJson<LocationRouteResponse>(request.downloadHandler.text);
            }
            else {
                Debug.Log("Failed: " + request.error);
            }
        }
    }
}
