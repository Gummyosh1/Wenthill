using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerHatAttack playerHatAttack;

    public void Start()
    {
        playerHatAttack = GetComponent<PlayerHatAttack>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerHatAttack.attack();
        }
    }
}
