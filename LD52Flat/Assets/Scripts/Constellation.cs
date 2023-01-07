using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[Serializable]
public struct Constellation
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public Vector2[] starPattern;
    [SerializeField]
    public int Size;
    [SerializeField]
    public ConstellationView ConstellationViewPrefab;

    public static Constellation GetScaledConstellation(Constellation source)
    {
        var constel=new Constellation();
        constel.Name = source.Name;
        constel.starPattern = new Vector2[source.starPattern.Length];
        constel.ConstellationViewPrefab=source.ConstellationViewPrefab;
        var ymax = source.starPattern[0].y;
        for (int i = 0; i < source.starPattern.Length; i++)
        {
            if (source.starPattern[i].y > ymax)
            {
                ymax = source.starPattern[i].y;
            }
        }
        ymax = Constellations.STARMAXY / ymax;
        for (int i = 0; i < source.starPattern.Length; i++)
        {
            constel.starPattern[i] = new Vector2 (source.starPattern[i].x * ymax, source.starPattern[i].y * ymax);
        }
        return constel;
    }

    public static Constellation[] ConstellationList;
    public static List<Constellation> SearchConstellations(List<StarView> stars, float accuracy)
    {
        Vector2[] coords =new Vector2[stars.Count];
        for (int i = 0; i < coords.Length; i++)
        {
            coords[i] = stars[i].Coords;
        }
        var ymin = coords[0].y;
        var idmin = 0;
        var ymax = 0f;
        for (int i = 0; i < coords.Length; i++)
        {
            if (coords[i].y < ymin)
            {
                idmin = i;
                ymin = coords[i].y;
            }
        }
        for (int i = 0; i < coords.Length; i++)
        {
            if (coords[i].y - coords[idmin].y > ymax)
            {
                ymax = coords[i].y - coords[idmin].y;
            }
        }
        ymax = Constellations.STARMAXY / ymax;
        var translatedCoords = new Vector2[coords.Length];
        for (var i = 0; i < coords.Length;i++)
        {
            translatedCoords[i] = new Vector2((coords[i].x - coords[idmin].x) * ymax, (coords[i].y - coords[idmin].y) * ymax);
        }
        var constellationsResult = new List<Constellation>();
        foreach (Constellation c in ConstellationList)
        {
            if (c.starPattern.Length != translatedCoords.Length) continue;
            var usedStars = new List<int>();

            var s = "translatedCoords ";
            for (int i = 0; i < translatedCoords.Length; i++)
                s += " (" + translatedCoords[i].x + " " + translatedCoords[i].y + " )";
            Debug.Log(s);
            s = "starPattern ";
            for (int i = 0; i < c.starPattern.Length; i++)
                s += " (" + c.starPattern[i].x + " " + c.starPattern[i].y + " )";
            Debug.Log(s);

            for (var i = 0; i < translatedCoords.Length;i++)
                for (var j = 0; j < c.starPattern.Length;j++) {
                    var dist = Vector2.Distance(translatedCoords[i], c.starPattern[j]);
                    if (dist < accuracy && !usedStars.Contains(j))
                    {
                        usedStars.Add(j);
                    }
                }
            if (usedStars.Count == c.starPattern.Length)
                constellationsResult.Add(c);
        }
        return constellationsResult;
    }

}
