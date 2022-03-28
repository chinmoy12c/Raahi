using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySingleton : MonoBehaviour
{
    public static UnitySingleton Instance { get; private set; }

    public Vector3 cameraPosition { get; set; }
    public int extendFromId { get; set;}
    public Vector3 extendFromVector { get; set; }
    public string locationData { get; set; }

    void Start()
    {
        extendFromId = -1;
        locationData = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
