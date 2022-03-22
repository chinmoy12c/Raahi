using UnityEngine;
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

    private Camera mainCamera;
    private List<GameObject> pathNodes;

	void Start () {
		mainCamera = Camera.main;
        pathNodes = new List<GameObject>();
	}

	public void placeTurn() {
        Vector3 poiLocation = mainCamera.transform.position + mainCamera.transform.forward * POI_DRAW_DISTANCE;
        GameObject pathNode = Instantiate(turnPointer, poiLocation, Quaternion.identity);
        pathNodes.Add(pathNode);
        if (pathNodes.Count >= 2) {
            Vector3 from = pathNodes[pathNodes.Count - 2].transform.position;
            Vector3 to = pathNodes[pathNodes.Count - 1].transform.position;
            drawPath(from, to);            
        }
    }

    public void placePoi() {
        Vector3 poiLocation = mainCamera.transform.position + mainCamera.transform.forward * POI_DRAW_DISTANCE;
        GameObject pathNode = Instantiate(poiPointer, poiLocation, Quaternion.identity);
        pathNodes.Add(pathNode);
        if (pathNodes.Count >= 2) {
            Vector3 from = pathNodes[pathNodes.Count - 2].transform.position;
            Vector3 to = pathNodes[pathNodes.Count - 1].transform.position;
            drawPath(from, to);            
        }
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
}