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

    public static Constellation GetScaledConstellation(Constellation source)
    {
        var constel=new Constellation();
        constel.Name = source.Name;
        constel.starPattern = new Vector2[source.starPattern.Length];
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
    public static List<Constellation> SearchConstellations(Vector2[] coords, float accuracy)
    {
        var ymin = coords[0].y;
        var idmin = 0;
        var ymax = coords[0].y;
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
            if (coords[i].y > ymax)
            {
                ymax = coords[i].y;
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
            for (var i = 0; i < translatedCoords.Length;i++)
                for (var j = 0; j < c.starPattern.Length;j++)
                    if (Vector2.Distance(translatedCoords[i], c.starPattern[j]) < accuracy && !usedStars.Contains(j))
                    {
                        usedStars.Add(j);
                    }
            if (usedStars.Count == c.starPattern.Length)
                constellationsResult.Add(c);
        }
        return constellationsResult;
    }
}
