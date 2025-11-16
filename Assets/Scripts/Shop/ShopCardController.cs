using UnityEngine;

public class ShopCardController : MonoBehaviour
{
    public void ScaleUp()
    {
        gameObject.transform.localScale *= 1.1f;
    }

    public void ScaleDown()
    {
        gameObject.transform.localScale = new Vector3(6f, 10f, 10f);
    }
}
