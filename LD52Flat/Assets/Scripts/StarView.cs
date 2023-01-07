using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarView : MonoBehaviour
{
    public bool Active;
    public Vector2 Coords;
    public SpriteRenderer SpriteRenderer;
    public Action<StarView> OnStarClick = delegate { };
    public Color DefaultColor;
    public Color SelectedColor;
    public Color InactiveColor;
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
        SpriteRenderer.color= SelectedColor;
    }

    public void Deselect()
    {
        SpriteRenderer.color = DefaultColor;
    }

    public void Deactivate()
    {
        SpriteRenderer.color = InactiveColor;
    }
}
