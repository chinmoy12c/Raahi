using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBehaviour : MonoBehaviour
{
    [SerializeField]
    private float ROTATIONS_PER_MIN = 120.0f;
    [SerializeField]
    private float TRANSLATION_RANGE = 0.2f;
    private Vector3 initialPosition;
    private Transform objectTransform;

    public int nodeId;

    void Start()
    {
        objectTransform = GetComponent<Transform>();
        initialPosition = objectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        objectTransform.Rotate(0f, 6.0f * ROTATIONS_PER_MIN * Time.deltaTime, 0f);
        objectTransform.position = new Vector3(
            initialPosition.x,
            initialPosition.y + TRANSLATION_RANGE * (float)Math.Sin(Time.time),
            initialPosition.z
        );
    }

    public int getNodeId() {
        return nodeId;
    }

    public void setNodeId(int nodeId) {
        this.nodeId = nodeId;
    }
}
