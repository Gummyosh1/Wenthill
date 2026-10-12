using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopObject;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopObject.SetActive(true);
        }
    }
}
