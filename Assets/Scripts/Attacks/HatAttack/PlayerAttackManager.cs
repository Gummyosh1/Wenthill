using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerHatAttack playerHatAttack;
    public GameObject shop;

    public void Start()
    {
        playerHatAttack = GetComponent<PlayerHatAttack>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !shop.activeSelf)
        {
            playerHatAttack.attack();
        }
    }
}
