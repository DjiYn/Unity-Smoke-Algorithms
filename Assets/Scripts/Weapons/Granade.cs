using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float delay = 3f;

    float countdown;
    bool hasExploded = false;

    Voxelizer voxelizer;

    void Start()
    {
        countdown = delay;
        voxelizer = GameObject.FindGameObjectWithTag("Voxel Grid").GetComponent<Voxelizer>();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            
            hasExploded = true;
        }
    }

    private void Explode()
    {
        voxelizer.CreateSmoke(transform);
        Destroy(gameObject);
    }
}
