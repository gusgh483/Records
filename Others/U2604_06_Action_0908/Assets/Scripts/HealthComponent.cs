using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    private float health;

    [SerializeField]
    private GameObject healthPrefab;

    public bool IsDead => health <= 0.0f;

    private GameObject healthObject;
    private Image uiImage;


#if UNITY_EDITOR
    private void Reset()
    {
        healthPrefab = Resources.Load<GameObject>("Prefabs/HealthBar");
        if (healthPrefab == null)
            Debug.LogWarning("Prefabs/HealthBar 에셋 없음");
    }
#endif

    private void Start()
    {
        healthObject = Instantiate<GameObject>(healthPrefab, transform);
        Image[] images = healthObject.transform.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if(image.gameObject.name == "Image_Bar")
            {
                uiImage = image;
                
                break;
            }
        }

        Character character = GetComponent<Character>();
        health = character.Data.MaxHealth;
        uiImage.fillAmount = health / character.Data.MaxHealth;
    }

    public void Damaged(float power)
    {
        Character character = GetComponent<Character>();

        health -= power;
        health = Mathf.Clamp(health, 0.0f, character.Data.MaxHealth);

        uiImage.fillAmount = health / character.Data.MaxHealth;
    }

    public void Dead()
    {
        Destroy(healthObject);
    }
}
