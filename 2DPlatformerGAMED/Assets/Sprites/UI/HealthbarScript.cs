using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{

    [SerializeField] private Slider _healthBar;


    public void SetHealthBar(float value)
    {
        _healthBar.value = value;
    }
}
