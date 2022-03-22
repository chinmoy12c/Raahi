using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehaviour : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Transform objectTransform;
    private Camera mainCamera;

    void Start()
    {
        objectTransform = GetComponent<Transform>();
        meshRenderers = objectTransform.GetComponentsInChildren<MeshRenderer>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(mainCamera.transform.position, objectTransform.position) > 3)
            changeVisibility(meshRenderers, false);
        else
            changeVisibility(meshRenderers, true);
    }

    void changeVisibility(MeshRenderer[] meshRenderers, bool enabled) {
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.enabled = enabled;
    }
}
