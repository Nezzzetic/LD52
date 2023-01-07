using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarView : MonoBehaviour
{
    public Vector2 Coords;
    public SpriteRenderer SpriteRenderer;
    public Action<StarView> OnStarClick = delegate { };
    private void OnMouseDown()
    {
        OnStarClick(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        SpriteRenderer.color= Color.yellow;
    }

    public void Deselect()
    {
        SpriteRenderer.color = Color.black;
    }
}
