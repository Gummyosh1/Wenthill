using System.Collections;
using UnityEditor;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject newTowerPrefab;
    private GameObject newTower;
    private SpriteRenderer newTowerSpriteRenderer;
    public GameObject shopWindow;
    private bool isTowerPlacing = false;

    private float timer = 0;
    private float redTime = 2f;

    public void towerHover()
    {
        shopWindow.SetActive(false);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = transform.position.z;

        newTower = Instantiate(newTowerPrefab, worldPosition, Quaternion.identity);
        newTowerSpriteRenderer = newTower.GetComponent<SpriteRenderer>();

        isTowerPlacing = true;
    }

    public void Update()
    {
        if (isTowerPlacing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool canPlace = true;

                // World-space half-width of the preview tower.
                float halfWidth = newTowerSpriteRenderer.bounds.extents.x;

                // Convert that width into a screen-space offset.
                Vector3 centerScreen =
                    mainCamera.WorldToScreenPoint(newTowerSpriteRenderer.bounds.center);

                Vector3 rightScreen = mainCamera.WorldToScreenPoint(
                    newTowerSpriteRenderer.bounds.center + Vector3.right * halfWidth
                );

                Vector3 screenOffset = rightScreen - centerScreen;
                screenOffset.z = 0f;

                for (int i = -1; i <= 1; i++)
                {
                    Vector3 checkPosition = Input.mousePosition + screenOffset * i;
                    Ray ray = mainCamera.ScreenPointToRay(checkPosition);
                    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

                    foreach (RaycastHit2D hit in hits)
                    {
                        // Ignore the preview tower and its children.
                        if (hit.collider.transform == newTower.transform ||
                            hit.collider.transform.IsChildOf(newTower.transform))
                        {
                            continue;
                        }

                        foreach (Transform child in
                                hit.collider.GetComponentsInChildren<Transform>())
                        {
                            if (child.CompareTag("Tower"))
                            {
                                canPlace = false;
                            }
                        }
                    }
                }

                if (canPlace)
                {
                    isTowerPlacing = false;
                }
                else
                {
                    StartCoroutine(cantPlaceAnimation());
                }

                
            }
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = transform.position.z - mainCamera.transform.position.z;

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = transform.position.z;

            newTower.transform.position = new Vector3 (worldPosition.x,-1,worldPosition.z);
        }
    }


    public IEnumerator cantPlaceAnimation()
    {
        float transitionTime = 0.08f;
        Color startColor = newTowerSpriteRenderer.color;
        Color red = new Color(1f, 0f, 0f, startColor.a);

        // Quickly fade to red.
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            newTowerSpriteRenderer.color = Color.Lerp(startColor, red, t / transitionTime);
            yield return null;
        }

        newTowerSpriteRenderer.color = red;

        // Fade back to the normal color.
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            newTowerSpriteRenderer.color = Color.Lerp(red, startColor, t / transitionTime);
            yield return null;
        }

        newTowerSpriteRenderer.color = startColor;
    }
}
