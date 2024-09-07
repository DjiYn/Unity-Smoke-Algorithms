using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalRenderData", menuName = "ScriptableObjects/GlobalRenderData", order = 1)]
public class GlobalRenderData : ScriptableObject
{
    public Raymarcher raymarcher;
    public string currentSceneName;
    public GameObject voxelizer;
}
