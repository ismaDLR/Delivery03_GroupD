using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject Health;

    private RectTransform rectHealth;

    private void OnEnable()
    {
        InventorySlotUI.OnChangeHealth += RestoreHealth;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnChangeHealth -= RestoreHealth;
    }

    private void Start()
    {
        rectHealth = Health.GetComponent<RectTransform>();
    }

    public void OnClickHealth()
    {
        RaycastHit2D hitData = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hitData)
        {
            if (hitData.collider.gameObject.tag == "Health")
            {
                rectHealth.sizeDelta = new Vector2(rectHealth.sizeDelta.x - 0.05f, rectHealth.sizeDelta.y);
            }
        }
    }

    public void RestoreHealth(int health)
    {
        Debug.Log(health);
        rectHealth.sizeDelta = new Vector2(rectHealth.sizeDelta.x + ((float)health / 100), rectHealth.sizeDelta.y);

        if (rectHealth.sizeDelta.x > 1)
        {
            rectHealth.sizeDelta = new Vector2(1, rectHealth.sizeDelta.y);
        }
    }
}
