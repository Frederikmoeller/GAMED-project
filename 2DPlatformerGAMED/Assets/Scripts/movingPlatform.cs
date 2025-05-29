using UnityEditorInternal;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    [SerializeField] private float _speed = 0.1f, _swtichTime = 10f, _timKeep;

    // Update is called once per frame
    void Update()
    {
        _timKeep += Time.deltaTime;
        if (_timKeep >= _swtichTime)
        {
            _timKeep = 0;
            _speed = -_speed;
        }
        Vector2 direction = new Vector2(_speed, 0f);
        transform.Translate(direction * Time.deltaTime);
    }
}
