using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyStar : MonoBehaviour
{

    public Vector3 destination;
    public Vector3 Movement;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        Destroy(gameObject,2);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
