using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour {

	[SerializeField]
    private LocationScanner locationScanner;
	[SerializeField]
    private LocationTracker locationTracker;
	[SerializeField]
	private InputField trackingLocationField;
	[SerializeField]
	private InputField scanningLocationField, scanningPOIField;
	[SerializeField]
	private Dropdown poiSelector;

	void LateUpdate () {
		// if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
		// 	RaycastHit hit;
		// 	Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			
		// 	if (Physics.Raycast(ray, out hit)) {
		// 		Transform objectHit = hit.transform;
		// 		Debug.Log(objectHit.transform.position);
		// 	}
		// }

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "extendPath") {
				UnitySingleton.Instance.cameraPosition = Camera.main.transform.position;
				UnitySingleton.Instance.extendFromId = hit.transform.gameObject.GetComponent<SignBehaviour>().getNodeId();
				UnitySingleton.Instance.extendFromVector = hit.transform.gameObject.transform.parent.position;
				SceneManager.LoadScene(sceneName:"ScanningScene");
			}
		}
	}

	public void dropTurnOnClick(){
		locationScanner.placeTurn();
	}

	public void dropPoiOnClick() {
		locationScanner.placePoi(scanningLocationField.text, scanningPOIField.text);
	}

	public void launchScanningScene() {
		SceneManager.LoadScene(sceneName:"ScanningScene");
	}

	public void launchTrackingScene() {
		SceneManager.LoadScene(sceneName:"TrackingScene");
	}

	public void resolveLocationOnClick () {
		string locationName = trackingLocationField.text;
		locationTracker.fetchLocation(locationName);
	}

	public void drawRouteOnClick() {
		locationTracker.drawAllRoutes();
	}

	public void onPoiSelected() {
		locationTracker.drawRoute(poiSelector.options[poiSelector.value].text);
	}
}