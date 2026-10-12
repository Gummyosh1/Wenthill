using UnityEngine;

public class PlayerHatAttack : MonoBehaviour
{
    private PlayerMove playerMove;
    public GameObject hatPrefab;
    private Transform throwPosition;

    public void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        throwPosition = GameObject.Find("HatThrowPoint").transform;
    }

    public void attack()
    {
        GameObject hat = Instantiate(hatPrefab, throwPosition.position, Quaternion.identity);

        hat.GetComponent<HatProjectile>().right = playerMove.getIsFacingRight();
    }
}
