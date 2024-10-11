using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobUpDownUI : MonoBehaviour
{
    public float bobSpeed = 2f; // Speed of the bobbing motion
    public float bobHeight = 10f; // Height of the bobbing motion (UI uses smaller units)

    private Vector3 startPosition;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component
        startPosition = rectTransform.localPosition; // Store the local position of the UI element
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position based on sine wave, relative to the startPosition
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        // Apply the new position, keeping the initial X and Z position the same
        rectTransform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
