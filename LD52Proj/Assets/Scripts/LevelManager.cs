using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{

    public float accuracy;
    public Vector2[] points;
    
    public Constellations ConstellationsData;
    // Start is called before the first frame update
    void Start()
    {
        Constellation.ConstellationList = new Constellation[ConstellationsData.ConstellationList.Length];
        for (var i = 0; i < ConstellationsData.ConstellationList.Length; i++)
        {
            Constellation.ConstellationList[i] = Constellation.GetScaledConstellation(ConstellationsData.ConstellationList[i]);
        }
        foreach (var constellation in Constellation.ConstellationList)
        {
            Debug.Log("Loaded constellations");
            Debug.Log(constellation.Name + " ");
            for (int i = 0; i < constellation.starPattern.Length; i++)
                Debug.Log(" (" + constellation.starPattern[i].x + " "+ constellation.starPattern[i].y+ " )");
            
        }
            
    
}

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Check()
    {
        var constellations = Constellation.SearchConstellations(points,accuracy);
        if (constellations != null) { 
            foreach (var constellation in constellations)
            Debug.Log(constellation.Name); } 
        else Debug.Log("No constellation");
    }
}
