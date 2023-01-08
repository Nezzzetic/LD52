using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ConstellationPanel : MonoBehaviour
{
    Constellation _constellation;
    public Image Back;
    public Transform StarsParent;
    public GameObject MinistarsPrefab;
    public GameObject Done;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Constellation cons)
    {
        _constellation=cons;
        var mod = 0f;

        foreach (Vector2 pos in cons.starPattern)
        {
            if (pos.x < mod) mod = pos.x;
        }
        var multy = 1f;
        var xmax = 0f;
        foreach (Vector2 pos in cons.starPattern)
        {
            var x = (pos.x - mod) / 5;
            if (Mathf.Abs(x) > xmax)
            {
                xmax = Mathf.Abs(x);
            }
        }
        if (xmax>200) { 
        multy = 200 / xmax;
        }
        Debug.Log("modmodmod" + mod);
        Debug.Log("multy" + multy);
        foreach (Vector2 pos in cons.starPattern)
        {
            var star = Instantiate(MinistarsPrefab, StarsParent);
            Debug.Log("pospospospospos" + pos);
            var x = (pos.x - mod) / 5;
            var y = pos.y / 5;
            Debug.Log("xxx" + x+ "yyy" + y );
            //if (x>200)
            //{
            //    multy = 200 / x;
            //} else if(x<-200) {
            //    multy = - 200 / x;
            //}
            Debug.Log(x * multy+" " + y * multy);
            star.GetComponent<RectTransform>().localPosition = new Vector3(x * multy - 100, y * multy - 100, 0);
        }
    }

    public void Unblock()
    {
        Back.enabled= true;
        Back.sprite = _constellation.PanelSprite;
        Done.SetActive(true);
    }

    public void Undone()
    {
        Done.SetActive(false);
    }

}
