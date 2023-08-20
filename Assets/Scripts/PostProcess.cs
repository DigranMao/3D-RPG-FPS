using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    public PlayerController player;
    PostProcessVolume postProcess;
    Vignette vignette;

    public float vignetteSpeed = 2f;
    float increase;
    float decrease;

    void Start()
    {
        postProcess = GetComponent<PostProcessVolume>();
        postProcess.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        if(player.health <= 50)
        {
            vignette.intensity.value = 0.35f;
        }
    }
}
