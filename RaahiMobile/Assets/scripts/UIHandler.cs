using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour {

    public LocationScanner locationScanner;
	[SerializeField]
	private Button dropTurnButton;
	[SerializeField]
	private Button dropPoiButton;

	void Start () {
		dropTurnButton.onClick.AddListener(dropTurnOnClick);
		dropPoiButton.onClick.AddListener(dropPoiOnClick);
	}

	void dropTurnOnClick(){
		locationScanner.placeTurn();
	}

	void dropPoiOnClick() {
		locationScanner.placePoi();
	}
}