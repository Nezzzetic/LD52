using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationView : MonoBehaviour
{
    public Constellation Constellation;
    public SpriteRenderer SpriteRenderer;
    public Animator Animator;
    // Start is called before the first frame update
    void Awake()
    {
        Animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dissapear() {
        Animator.SetBool("dissapear", true);
    }
}
