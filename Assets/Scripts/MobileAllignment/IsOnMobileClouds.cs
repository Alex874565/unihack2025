using UnityEngine;

public class CloudAligner : MonoBehaviour
{
    // The fixed X scale value you want to send to the Animator
    private const float MobileScaleX = 2.3f; 
    
    // The animator component for this object
    private Animator _cloudAnimator;

    void Start()
    {
        // 1. Get the Animator component
        _cloudAnimator = GetComponent<Animator>();
        
        // Check: This code runs ONLY on the actual iPhone device (or Android).
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android)
        {
            // --- 1. Apply Position Change (Still applies to Transform) ---
            // Position is NOT usually controlled by the Animator, so this should work directly.
            float MobilePositionX = -2.3f; 
            Vector3 newPosition = transform.position;
            newPosition.x = MobilePositionX; 
            transform.position = newPosition;

            // --- 2. Apply SCALING Change via Animator Parameter ---
            if (_cloudAnimator != null)
            {
                // Set the float parameter defined in the Animator Controller
                _cloudAnimator.SetFloat("MobileScale", MobileScaleX); 
            }
        }
    }
}