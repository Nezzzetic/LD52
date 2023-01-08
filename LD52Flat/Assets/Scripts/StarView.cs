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
    public GameObject Countour;
    public Sprite InactiveSprite;

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
        Countour.SetActive(true);
    }

    public void Deselect()
    {
        Countour.SetActive(false);
    }

    public void Deactivate()
    {
        Countour.SetActive(false);
        SpriteRenderer.sprite = InactiveSprite;
    }
}
