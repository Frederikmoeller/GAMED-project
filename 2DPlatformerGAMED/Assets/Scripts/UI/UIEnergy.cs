using UnityEngine;

public class UIEnergy : MonoBehaviour
{
    [SerializeField] private GameObject _energy1, _energy2, _energy3,_allEnergy;
    private int energylvl = 3;


    public void changeEnergylvl(int energy)
    {
        energylvl = energy;

        switch (energylvl)
        {
            case 0:
                _energy3.gameObject.SetActive(false);
                break;
            case 1:
                _energy2.gameObject.SetActive(false);
                break;
            case 2:
                _energy1.gameObject.SetActive(false);
                break;
            case 3:
                _energy1.gameObject.SetActive(true);
                _energy2.gameObject.SetActive(true);
                _energy3.gameObject.SetActive(true);
                break;
        }
    }

    public void toggleEnergy(bool on)
    {
       
        _allEnergy.gameObject.SetActive(on);
        
    }

}
