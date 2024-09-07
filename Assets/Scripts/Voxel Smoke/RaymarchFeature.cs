using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RaymarchFeature : ScriptableRendererFeature
{
    RaymarchPass m_ScriptablePass;
    public GlobalRenderData globalRenderData;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new RaymarchPass(globalRenderData);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(m_ScriptablePass);
    }
}

