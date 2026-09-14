using UnityEngine;

public class PlayerTagUpdater : MonoBehaviour
{
    public CameraFollow cameraFollow;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BottomGround")
        {
            Debug.Log("Setting to false");
            cameraFollow.floorToPlatform = false;
            cameraFollow.platformToFloor = true;
        }

        if (collision.gameObject.tag == "OneWayPlatform") //INSERT NEW PLATFORM TYPES HERE
        {
            Debug.Log("Setting to true");
            cameraFollow.floorToPlatform = true;
            cameraFollow.platformToFloor = false;
        }
    }
}
