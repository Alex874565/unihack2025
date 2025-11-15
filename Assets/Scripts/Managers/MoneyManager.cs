using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public float CurrentMoney => currentMoney;

    [SerializeField] private int startingMoney;

    private float currentMoney;

    private void Start()
    {
        currentMoney = startingMoney;
    }

    public void GainMoney(float amount)
    {
        currentMoney += amount;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentMoney >= amount;
    }

    public void SpendMoney(int amount)
    {
       currentMoney -= amount;
    }
}
