using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonURL : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenUrlWeb()
    {
        Application.OpenURL("https://site-ludus.vercel.app/");
        Debug.Log("Abriu o site");
    }

    public void OpenUrlInstagram()
    {
        Application.OpenURL("https://www.instagram.com/maisludus?igsh=MXgydzFldWRxc256Zg==");
        Debug.Log("Abriu o instagram");
    }
}
