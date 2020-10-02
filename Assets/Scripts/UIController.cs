using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text ammoText;


    public void updateAmmo(int ammoInMagazine, int totalAmmo)
    {

        ammoText.text = (ammoInMagazine + "/" + totalAmmo);
    }
}
