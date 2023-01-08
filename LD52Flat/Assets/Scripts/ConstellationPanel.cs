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

        foreach (Vector2 pos in cons.starPattern)
        {
            var star = Instantiate(MinistarsPrefab, StarsParent);
            Debug.Log(pos);
            var x = (pos.x - mod) / 5;
            var y = pos.y / 5;
            var multy = 1f;
            if (x>200)
            {
                multy = 200 / x;
            } else if(x<-200) {
                multy = - 200 / x;
            }
            Debug.Log(x * multy+" " + y * multy);
            star.GetComponent<RectTransform>().localPosition = new Vector3(x * multy - 100, y * multy - 100, 0);
        }
    }

    public void Unblock()
    {
        Back.enabled= true;
        Back.sprite = _constellation.ConstellationViewPrefab.SpriteRenderer.sprite;
    }

}
