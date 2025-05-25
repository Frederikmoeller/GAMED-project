using UnityEngine;

public class BossHP : MonoBehaviour
{

    [SerializeField] private int _bossHp = 3;

    public void damage()
    {
        _bossHp--;
        if (_bossHp == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
