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
        Back.sprite = cons.ConstellationViewPrefab.SpriteRenderer.sprite;
        foreach (Vector2 pos in cons.starPattern)
        {
            var star = Instantiate(MinistarsPrefab, StarsParent);
            Debug.Log(pos);
            star.GetComponent<RectTransform>().localPosition = new Vector3(pos.x / 5 - 100, pos.y / 5 - 100, 0);
        }

    }

}
