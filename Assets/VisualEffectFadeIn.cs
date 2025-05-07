using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectFadeIn : MonoBehaviour
{
    public VisualEffect vfx;
    public string exposedParameterName = "Spawn Rate";
    [Range(0, 1000)] public int spawnRate;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        vfx.SetUInt(exposedParameterName, (uint)spawnRate);
    }
}
