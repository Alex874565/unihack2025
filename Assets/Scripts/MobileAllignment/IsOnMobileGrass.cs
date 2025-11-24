using UnityEngine;

public class GrassAligner : MonoBehaviour
{
    // The desired scale factor for the X-axis on mobile
    private const float MobileScaleXFactor = 1.15f;

    void Start()
    {
        // Check if the game is running on a mobile platform
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android)
        {
            // Apply the scaling adjustment
            Vector3 currentScale = transform.localScale;
            currentScale.x *= MobileScaleXFactor; // Scales the current X by 1.15
            transform.localScale = currentScale;
        }
    }
}