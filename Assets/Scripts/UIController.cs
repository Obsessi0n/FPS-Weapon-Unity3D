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
        if (ammoInMagazine == -1 && totalAmmo == -1)
            ammoText.text = ("NA");
        else
            ammoText.text = (ammoInMagazine + "/" + totalAmmo);
    }
}
