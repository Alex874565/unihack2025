using UnityEngine;

public class CloudAligner : MonoBehaviour
{
    // The desired position and scale factor for mobile
    private const float MobilePositionX = -2.3f;
    private const float MobileScaleX = 2.3f; // Fixed X scale value

    void Start()
    {
        // Check: This code runs ONLY on the actual iPhone device (or Android).
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android)
        {
            // --- 1. Apply Position Change ---
            // Sets the X position to the fixed value of -2.3 on mobile.
            Vector3 newPosition = transform.position;
            newPosition.x = MobilePositionX; 
            transform.position = newPosition;

            // --- 2. Apply Scaling Change ---
            // Sets the X scale to the fixed value of 2.3 on mobile.
            Vector3 newScale = transform.localScale;
            newScale.x = MobileScaleX; 
            transform.localScale = newScale;
        }
    }
}