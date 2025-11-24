using UnityEngine;

public class WaterAligner : MonoBehaviour
{
    // The desired position and scale factor for mobile
    private const float MobilePositionX = -2.4f;
    private const float MobileScaleXFactor = 1.15f;

    void Start()
    {
        // Initial Check: This code will only run on the actual iPhone device (or Android device).
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android)
        {
            // --- 1. Apply Position Change ---
            // Sets the X position to the fixed value of -2.4 on mobile.
            Vector3 newPosition = transform.position;
            newPosition.x = MobilePositionX; 
            transform.position = newPosition;

            // --- 2. Apply Scaling Change ---
            // Scales the current X by 1.15 on mobile.
            Vector3 newScale = transform.localScale;
            newScale.x *= MobileScaleXFactor; 
            transform.localScale = newScale;
        }
    }
}