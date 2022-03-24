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
	private InputField locationField;

	public void dropTurnOnClick(){
		locationScanner.placeTurn();
	}

	public void dropPoiOnClick() {
		locationScanner.placePoi();
	}

	public void launchScanningScene() {
		SceneManager.LoadScene(sceneName:"ScanningScene");
	}

	public void launchTrackingScene() {
		SceneManager.LoadScene(sceneName:"TrackingScene");
	}

	public void resolveLocation() {
		string locationName = locationField.text;
		locationTracker.fetchLocation(locationName);
	}
}