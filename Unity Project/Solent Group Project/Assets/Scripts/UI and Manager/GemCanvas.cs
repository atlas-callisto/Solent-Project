using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCanvas : MonoBehaviour
{
    public GameObject gem0;
    public GameObject gem1;
    public GameObject gem2;
    public GameObject gem3;

    private static bool gem0Found = false;
    private static bool gem1Found = false;
    private static bool gem2Found = false;
    private static bool gem3Found = false;

    private void Start()
    {
        if(gem0Found) gem0.SetActive(true);
        if(gem1Found) gem1.SetActive(true);
        if(gem2Found) gem2.SetActive(true);
        if(gem3Found) gem3.SetActive(true);
    }
    public void UpdateCanvas(int gemNumber)
    {
        switch (gemNumber)
        {
            case 0:
                gem0Found = true;
                gem0.SetActive(true);
                break;
            case 1:
                gem1Found = true;
                gem1.SetActive(true);
                break;
            case 2:
                gem2Found = true;
                gem2.SetActive(true);
                break;
            case 3:
                gem3Found = true;
                gem3.SetActive(true);
                break;
            default:
                break;
        }
    }
}
