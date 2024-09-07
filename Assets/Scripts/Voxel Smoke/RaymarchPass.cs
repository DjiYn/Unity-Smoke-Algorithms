using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class RaymarchPass : ScriptableRenderPass
{
    private Raymarcher raymarcher;

    GlobalRenderData globalRenderData;

    int tintId = Shader.PropertyToID("_Tempo");
    RenderTargetIdentifier source, tint;

    public RaymarchPass()
    {
        raymarcher = GameObject.FindObjectOfType<Raymarcher>();
    }
    public RaymarchPass(GlobalRenderData globalRenderData)
    {
        this.globalRenderData = globalRenderData;
    }


    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureInput(ScriptableRenderPassInput.Depth);
        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.GetTemporaryRT(tintId, desc, FilterMode.Bilinear);
        tint = new RenderTargetIdentifier(tintId);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(tintId);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        raymarcher = globalRenderData.raymarcher;

        CommandBuffer cmd = CommandBufferPool.Get("RayMarch");

        if (Application.isPlaying && raymarcher != null)
        {
            raymarcher.RenderSmoke(cmd, source, tint);
        }
       
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
