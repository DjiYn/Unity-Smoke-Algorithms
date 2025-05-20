using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RaymarchCustomPass : CustomPass
{
    public Raymarcher raymarcher;
    public GlobalRenderData globalRenderData;

    int tintId = Shader.PropertyToID("_Tempo");
    RTHandle tint;
    RenderTexture colorTexture;

    public RaymarchCustomPass(GlobalRenderData globalRenderData)
    {
        this.globalRenderData = globalRenderData;
    }

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        // Setup is called once when the pass is initialized.
        // Allocate the temporary RT used for rendering.
        tint = RTHandles.Alloc(Vector2.one, TextureXR.slices, dimension: TextureDimension.Tex2D,
                               useDynamicScale: true, name: "TintTexture");

        var descriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.ARGB32);
        colorTexture = new RenderTexture(descriptor);
        colorTexture.Create();
    }

    protected override void Execute(CustomPassContext ctx)
    {
        raymarcher = globalRenderData.raymarcher;

        // Create the command buffer
        CommandBuffer cmd = ctx.cmd;

        cmd.GetTemporaryRT(tintId, ctx.cameraColorBuffer.rt.width, ctx.cameraColorBuffer.rt.height, 0, FilterMode.Bilinear);

        var tint = new RenderTargetIdentifier(tintId);

        CoreUtils.SetRenderTarget(ctx.cmd, colorTexture);
        ctx.cmd.Blit(ctx.cameraDepthBuffer, colorTexture);

        if (Application.isPlaying && raymarcher != null)
        {
            // Execute the raymarch rendering logic
            raymarcher.RenderSmoke(cmd, colorTexture, ctx.cameraColorBuffer);
        }
    }

    protected override void Cleanup()
    {
        // Cleanup is called when the pass is destroyed.
        // Release the temporary RT.
        RTHandles.Release(tint);
    }
}
