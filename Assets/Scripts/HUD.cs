using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Slider waterSlider;
    [SerializeField] private Slider soilSlider;
    [SerializeField] private Slider airSlider;
    [SerializeField] private Button shopButton;

    private void Awake()
    {
        shopButton.onClick.AddListener(() =>
        {
            //show shop
        });

        
    }

}
