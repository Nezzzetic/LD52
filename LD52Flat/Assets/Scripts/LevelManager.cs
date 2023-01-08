using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public StarView StarPrefab;
    public FlyStar FlyStarPrefab;
    public Transform FlyStarPrefabParent;
    public ConstellationPanel ConstellationPanelPrefab;
    public ConstellationShopPanel ConstellationShopPanelPrefab;
    public Transform ConstellationPanelParent;
    public Transform ConstellationShopPanelParent;
    public List<StarView> Stars=new List<StarView>();
    public List<StarView> SelectedStars = new List<StarView>();
    public List<ConstellationView> ConstellationViews = new List<ConstellationView>();
    public List<ConstellationPanel> ConstellationPanels = new List<ConstellationPanel>();
    public float accuracy;
    public Vector2 MinPointOnScene;
    public Vector2 MaxPointOnScene;
    public Vector2Int FieldSize;
    public Vector2 FieldDelta;
    public GameObject ButtonToCreate;
    public int Points;
    public int NewConstelPoints;
    public float SSize;
    public float MSize;
    public float LSize;

    public GameObject ButtonToHarvest;
    public GameObject ButtonToNigth;
    public GameObject DayUI;
    public GameObject NightUI;
    public TMP_Text PointsText;
    public List<Constellation> UsedConstel = new List<Constellation>();

    public Constellations ConstellationsData;
    public Animator DayAnimator;
    public GameObject WinScreen;
    public bool WinScreenShowed;

    private float _harvestTimer;
    private float _nightTimer;
    // Start is called before the first frame update
    void Start()
    {
        _loadConstellation();
        FieldDelta = new Vector2((MaxPointOnScene.x - MinPointOnScene.x) / FieldSize.x, (MaxPointOnScene.y - MinPointOnScene.y) / FieldSize.y);
        ButtonToCreate.SetActive(false);
        ChangePoints(0);
        _createStarsStart();
        DayUI.SetActive(false);
        ButtonToNigth.SetActive(false);
        _nightTimer = 3;
        WinScreenShowed = false;
    }

    public void ShowWinScreen()
    {
        if (!WinScreenShowed)
        {
            WinScreenShowed = true;
            WinScreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_harvestTimer>0)
        {
            _harvestTimer-=Time.deltaTime;
            if (_harvestTimer<=0) HarvestEnded();
        }
        if (_nightTimer>0)
        {
            _nightTimer-=Time.deltaTime;
            if (_nightTimer<=0) NightStarted();
        }
    }
    public void HarvestStart()
    {
        ButtonToHarvest.SetActive(false);
        Harvest();
        _harvestTimer=2;
        
    }
    public void NightStart()
    {
        ResetStars();
        DayUI.SetActive(false);
        ButtonToNigth.SetActive(false);
        _nightTimer = 3;
        DayAnimator.SetBool("day", false);
        foreach (var item in ConstellationPanels)
        {
            item.Undone();
        }
        
    }
    public void HarvestEnded()
    {
        DayAnimator.SetBool("day", true);
        NightUI.SetActive(false);
        DayUI.SetActive(true);
        ButtonToNigth.SetActive(true);
    }

    public void NightStarted()
    {
        NightUI.SetActive(true);
    }
    public void CheckAndMoveButton()
    {
        if (SelectedStars.Count == 0) return;
        var constellations = Constellation.SearchConstellations(SelectedStars, accuracy);
        if (constellations.Count > 0)
        {
            foreach (var constellation in constellations)
                Debug.Log(constellation.Name);
            var constel = constellations[0];
            if (UsedConstel.Contains(constel))
            {
                ButtonToCreate.SetActive(false);
                Debug.Log("used!");
            } else if (constel.State==0)
            {
                ButtonToCreate.SetActive(false);
                Debug.Log("not owned!");
            }
            else
            {
                var size = getSize(SelectedStars);
                if (constel.Size==0 && size>SSize)
                {
                    Debug.Log("bad size!");
                }
                else if (constel.Size == 1 && size > MSize)
                {
                    ButtonToCreate.SetActive(false);
                    Debug.Log("bad size!");
                } else { 
                var pos = getCenterOfConstellation(SelectedStars);
                ButtonToCreate.SetActive(true);
                ButtonToCreate.GetComponent<RectTransform>().localPosition = new Vector3(pos.x/FieldDelta.x, pos.y/FieldDelta.y, 0);
                }
            }
        }
        else
        {
            ButtonToCreate.SetActive(false);
            Debug.Log("No constellation");
        }
    }

    public void Check()
    {
        if (SelectedStars.Count == 0) return;
        var constellations = Constellation.SearchConstellations(SelectedStars, accuracy);
        if (constellations.Count > 0)
        {
            foreach (var constellation in constellations)
                Debug.Log(constellation.Name);
            var constel = constellations[0];
            if (UsedConstel.Contains(constel))
            {
                Debug.Log("used!");
                foreach (StarView star in SelectedStars)
                {
                    star.Deselect();
                }
            }
            else if (constel.State == 0)
            {
                ButtonToCreate.SetActive(false);
                foreach (StarView star in SelectedStars)
                {
                    star.Deselect();
                }
            }
            else
            {
                _createConstellation(constel, SelectedStars);
                UsedConstel.Add(constel);
                constel.State++;
                constel.CreateAction();
                var starsInsideZone = _findAllStarsInside(SelectedStars);
                ButtonToHarvest.SetActive(true);
                foreach (StarView star in starsInsideZone)
                {
                    DeactStar(star);
                }
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
        ButtonToCreate.SetActive(false);
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
            
            var shopPanel = Instantiate(ConstellationShopPanelPrefab, ConstellationShopPanelParent);
            shopPanel.Init(constellation);
            shopPanel.OnShopPanelClick += ShopPanelClick;
            if (constellation.State>0) { 
                CreatePanel(constellation);
            }
        }
    }

    void _createStars()
    {
        for (var i = 0; i < 50; i++)
        {
            var rndx = UnityEngine.Random.Range(0, FieldSize.x);
            var rndy = UnityEngine.Random.Range(0, FieldSize.y);
            var rndr = UnityEngine.Random.Range(0, 360);
            var rndsc = UnityEngine.Random.Range(0.5f, .8f);

            var star = Instantiate(StarPrefab);
            star.Coords = new Vector2(rndx, rndy);
            star.transform.position = new Vector3(MinPointOnScene.x + FieldDelta.x * rndx, MinPointOnScene.y + FieldDelta.y * rndy);
            star.transform.Rotate(0, 0, rndr);
            star.transform.localScale = Vector3.one * rndsc * rndsc;
            Stars.Add(star);
            star.OnStarClick += OnStarClickAction;
            star.Active = true;
        }
    }

    void _createStarsStart()
    {
        
            var rndx = 600;
            var rndy = 400;
            var rndr = UnityEngine.Random.Range(0, 360);

            var star = Instantiate(StarPrefab);
            star.Coords = new Vector2(rndx, rndy);
            star.transform.position = new Vector3(MinPointOnScene.x + FieldDelta.x * rndx, MinPointOnScene.y + FieldDelta.y * rndy);
            star.transform.Rotate(0, 0, rndr);
            Stars.Add(star);
            star.OnStarClick += OnStarClickAction;
            star.Active = true;

         rndx = 800;
         rndy = 600;
         rndr = UnityEngine.Random.Range(0, 360);

        var star2 = Instantiate(StarPrefab);
        star2.Coords = new Vector2(rndx, rndy);
        star2.transform.position = new Vector3(MinPointOnScene.x + FieldDelta.x * rndx, MinPointOnScene.y + FieldDelta.y * rndy);
        star2.transform.Rotate(0, 0, rndr);
        Stars.Add(star2);
        star2.OnStarClick += OnStarClickAction;
        star2.Active = true;

    }

    public void Harvest()
    {
        int harvested = 0;
        SelectedStars.Clear();
        foreach (StarView star in Stars)
        {
            if (!star.Active) { harvested++;
                CreateFlyText(star.transform);
            }
            DestroyStar(star);
        }
        Stars.Clear();
        foreach (ConstellationView cons in ConstellationViews)
        {
            if (cons.Constellation.State == 2)
            {
                cons.Constellation.State ++;
            }
            cons.Dissapear();
            Destroy(cons.gameObject, 2);
        }
        ConstellationViews.Clear();
        UsedConstel.Clear();
        
        ChangePoints(harvested);
    }

    public void ResetStars()
    {
        
        _createStars();
    }

    void OnStarClickAction(StarView star)
    {
        if (!star.Active) { return; }
        
        if (SelectedStars.Contains(star))
        {
            SelectedStars.Remove(star);
            star.Deselect();
        } else
        {
            SelectedStars.Add(star);
            star.Select();
        }
        CheckAndMoveButton();
    }

    void DestroyStar(StarView star)
    {
        Destroy(star.gameObject,2);
        star.Dissapear();
    }

    void DeactStar(StarView star)
    {
        star.Active = false;
        star.Deactivate();
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
        conView.Constellation = con;


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

    float getSize(List<StarView> stars)
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
        return Math.Abs( (maxx - minx) * (maxy - miny));
    }

    List<StarView> _findAllStarsInside(List<StarView> stars)
    {
        var result = new List<StarView>();
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
        foreach (var view in Stars)
        {
            if (view.transform.position.x > maxx) continue;
            if (view.transform.position.y > maxy) continue;
            if (view.transform.position.x < minx) continue;
            if (view.transform.position.y < miny) continue;
            result.Add(view);
        }
        return result;
    } 
     public void ChangePoints(int delta)
    {
        Points += delta;
        PointsText.text=Points.ToString();
    }
    private int shopCounter = 13;
    public void ShopPanelClick(ConstellationShopPanel shopPanel)
    {
        if (shopPanel.Constellation.Cost > Points) return;
        ChangePoints(-shopPanel.Constellation.Cost);
        shopPanel.Constellation.State++;
        shopPanel.Solded();
        var panel = CreatePanel(shopPanel.Constellation);
        shopCounter--;
        if (shopCounter==0)
        {
            ShowWinScreen();
        }
    }

    public ConstellationPanel CreatePanel(Constellation constellation)
    {
            var panel = Instantiate(ConstellationPanelPrefab, ConstellationPanelParent);
            panel.Init(constellation);
        ConstellationPanels.Add(panel);
        constellation.CreateAction += delegate { UnblockPanel(panel); };
        return panel;
    }


    public void UnblockPanel(ConstellationPanel panel)
    {
        panel.Unblock();
    }

    public void CreateFlyText(Transform target)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        var fly = Instantiate(FlyStarPrefab, FlyStarPrefabParent);
        fly.transform.position = screenPos;
        Destroy(fly.gameObject,2);
    }

}
