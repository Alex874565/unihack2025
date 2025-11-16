using UnityEngine;

public class ShopCardController : MonoBehaviour
{
    private AudioManager audioManager;
    private bool hasHovered = false; // optional: avoid playing repeatedly

    private void Awake()
    {
        audioManager = ServiceLocator.Instance.AudioManager;
    }

    public void ScaleUp()
    {
        if (!ServiceLocator.Instance.TutorialManager.InTutorial)
        {
            gameObject.transform.localScale *= 1.1f;

            // Play hover sound once per hover
            if (!hasHovered && audioManager != null && audioManager.hover != null)
            {
                audioManager.PlaySFX(audioManager.hover);
                hasHovered = true;
            }
        }
    }

    public void ScaleDown()
    {
        gameObject.transform.localScale = new Vector3(6f, 10f, 10f);
        hasHovered = false; // reset so it can play next time
    }
}
