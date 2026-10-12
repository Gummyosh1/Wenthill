using System;
using UnityEngine;

public class PlayerHatAttack : MonoBehaviour
{
    private PlayerMove playerMove;
    public GameObject hatPrefab;
    [NonSerialized] public GameObject hatInstance;
    private Transform throwPosition;

    public void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        throwPosition = GameObject.Find("HatThrowPoint").transform;
    }

    public void attack()
    {
        if (hatInstance == null)
        {
            hatInstance = Instantiate(hatPrefab, throwPosition.position, Quaternion.identity);
            hatInstance.GetComponent<HatProjectile>().right = playerMove.getIsFacingRight();
        }
    }
}
