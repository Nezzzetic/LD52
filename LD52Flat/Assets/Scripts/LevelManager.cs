using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{
    public StarView StarPrefab;
    public List<StarView> Stars=new List<StarView>();
    public List<StarView> SelectedStars = new List<StarView>();
    public float accuracy;
    public Vector2 MinPointOnScene;
    public Vector2 MaxPointOnScene;
    public Vector2Int FieldSize;
    public Vector2 FieldDelta;

    public Constellations ConstellationsData;
    // Start is called before the first frame update
    void Start()
    {
        _loadConstellation();
        FieldDelta = new Vector2((MaxPointOnScene.x - MinPointOnScene.x) / FieldSize.x, (MaxPointOnScene.y - MinPointOnScene.y) / FieldSize.y);
        _createStars();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void Check()
    {
        var constellations = Constellation.SearchConstellations(SelectedStars, accuracy);
        if (constellations.Count > 0)
        {
            foreach (var constellation in constellations)
                Debug.Log(constellation.Name);
            foreach (StarView star in SelectedStars)
            {
                DestroyStar(star);
            }
        }
        else
        {
            Debug.Log("No constellation");
            foreach (StarView star in SelectedStars)
            {
                star.Deselect();
            }
        }
        SelectedStars.Clear();
    }

    void _loadConstellation()
    {
        Constellation.ConstellationList = new Constellation[ConstellationsData.ConstellationList.Length];
        for (var i = 0; i < ConstellationsData.ConstellationList.Length; i++)
        {
            Constellation.ConstellationList[i] = Constellation.GetScaledConstellation(ConstellationsData.ConstellationList[i]);
        }
        Debug.Log("Loaded constellations");
        foreach (var constellation in Constellation.ConstellationList)
        {
            var s = constellation.Name + " ";
            for (int i = 0; i < constellation.starPattern.Length; i++)
                s += " (" + constellation.starPattern[i].x + " " + constellation.starPattern[i].y + " )";
            Debug.Log(s);
        }
    }

    void _createStars()
    {
        for (var i = 0; i < 50; i++)
        {
            var rndx = UnityEngine.Random.Range(0, FieldSize.x);
            var rndy = UnityEngine.Random.Range(0, FieldSize.y);

            var star = Instantiate(StarPrefab);
            star.Coords = new Vector2(rndx, rndy);
            star.transform.position = new Vector3(MinPointOnScene.x + FieldDelta.x * rndx, MinPointOnScene.y + FieldDelta.y * rndy);
            Stars.Add(star);
            star.OnStarClick += OnStarClickAction;
        }
    }

    void OnStarClickAction(StarView star)
    {
        if (SelectedStars.Contains(star))
        {
            SelectedStars.Remove(star);
            star.Deselect();
        } else
        {
            SelectedStars.Add(star);
            star.Select();
        }
    }

    void DestroyStar(StarView star)
    {
        Destroy(star.gameObject);
    }


}
