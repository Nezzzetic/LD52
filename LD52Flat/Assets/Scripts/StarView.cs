using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarView : MonoBehaviour
{
    public bool Active;
    public bool Harvestable;
    public Vector2 Coords;
    public SpriteRenderer SpriteRenderer;
    public Action<StarView> OnStarClick = delegate { };
    public Color DefaultColor;
    public Color SelectedColor;
    public Color InactiveColor;
    public GameObject Countour;
    public Sprite InactiveSprite;
    public Animator Animator;
    private float shineTimer;
    private float nextShineTimer;

    private void OnMouseDown()
    {
        OnStarClick(this);
    }
    // Start is called before the first frame update
    void Awake()
    {
        nextShineTimer = UnityEngine.Random.Range(2f, 200f);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextShineTimer > 0)
        {
            nextShineTimer -= Time.deltaTime;
            if (nextShineTimer <= 0)
            {
                Animator.SetBool("shine", true);
                shineTimer = 2;
                nextShineTimer = UnityEngine.Random.Range(10f, 200f);
            }
        }
        if (shineTimer > 0)
        {
            shineTimer -= Time.deltaTime;
            if (shineTimer <= 0)
            {
                Animator.SetBool("shine", false);
            }
        }
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
        Animator.SetBool("active", false);
    }

    public void Dissapear()
    {
        Countour.SetActive(false);
        Animator.SetBool("dissapear", true);
    }
}
