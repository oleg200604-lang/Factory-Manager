using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MapScr : MonoBehaviour
{
    public int Width = 100;
    public int Height = 100;

    public List<ZoneScr> Zones = new();

    private void Start()
    {
        ZoneScr zone = new ZoneScr();

        Zones.Add(zone);

        Debug.Log(zone.B);      
        Debug.Log(zone.C);     
        Debug.Log(zone.Area); 
    }
}