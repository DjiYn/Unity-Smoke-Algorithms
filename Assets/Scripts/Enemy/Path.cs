using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();
    public List<Transform> Waypoints { get { return waypoints; } set { waypoints = value; } }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
