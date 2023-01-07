using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{
    public StarView StarPrefab;
    public ConstellationPanel ConstellationPanelPrefab;
    public Transform ConstellationPanelParent;
    public List<StarView> Stars=new List<StarView>();
    public List<StarView> SelectedStars = new List<StarView>();
    public List<ConstellationView> ConstellationViews = new List<ConstellationView>();
    public List<ConstellationPanel> ConstellationPanels = new List<ConstellationPanel>();
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
            var constel = constellations[0];
            _createConstellation(constel, SelectedStars);
            foreach (StarView star in SelectedStars)
            {
                DestroyStar(star);
                Stars.Remove(star);
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
            var panel = Instantiate(ConstellationPanelPrefab, ConstellationPanelParent);
            panel.Init(constellation);
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

    public void ResetStars()
    {
        SelectedStars.Clear();
        foreach (StarView star in Stars)
        {
            DestroyStar(star);
        }
        Stars.Clear();
        _createStars();
        foreach (ConstellationView cons in ConstellationViews)
        {
            Destroy(cons);
        }
        ConstellationViews.Clear();
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

    void _createConstellation(Constellation con,List<StarView> stars)
    {
        var conView = Instantiate(con.ConstellationViewPrefab);
        var pos = getCenterOfConstellation(stars);
        conView.transform.position = new Vector3(pos.x,pos.y,0);
        var y = conView.SpriteRenderer.bounds.size.y;
        Debug.Log(y);
        conView.transform.localScale = Vector3.one / y* pos.z;
        ConstellationViews.Add(conView);
        
    }

    Vector3 getCenterOfConstellation(List<StarView> stars)
    {
        var minx = stars[0].transform.position.x;
        var maxx = stars[0].transform.position.x;
        var miny = stars[0].transform.position.y;
        var maxy = stars[0].transform.position.y;
        foreach (var view in stars)
        {
            if (view.transform.position.x > maxx) maxx = view.transform.position.x;
            if (view.transform.position.y > maxy) maxy = view.transform.position.y;
            if (view.transform.position.x < minx) minx = view.transform.position.x;
            if (view.transform.position.y < miny) miny = view.transform.position.y;
        }
        return new Vector3((maxx-minx)/2+ minx, (maxy-miny)/2+ miny, maxy-miny);
    }



}
