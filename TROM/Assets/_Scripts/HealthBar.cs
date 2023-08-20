using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject hasHealthBarObject;
    public IHasHealthBar healthObject; // Set this in the inspector or in code
    public Slider slider; // The Slider UI element for the health bar
    public TextMeshProUGUI  healthText; // The Text UI element for the health number
    public Camera mainCamera; // Reference to the main camera

    void Start()
    {
        healthObject = hasHealthBarObject.GetComponent<IHasHealthBar>();
        mainCamera = Camera.main; // Assuming this script is run after the camera is created

        if(healthText) healthText.fontSharedMaterial.renderQueue = 4001;
    }
    void Update()
    {
        if (healthObject != null)
        {
            slider.maxValue = healthObject.MaxHealth;
            slider.value = healthObject.CurrentHealth;

            if(healthText) healthText.text = $"{healthObject.CurrentHealth}/{healthObject.MaxHealth}";
        }
    }
}

public interface IHasHealthBar
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }
}