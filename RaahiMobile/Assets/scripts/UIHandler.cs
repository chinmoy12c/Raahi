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
}