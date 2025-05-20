Shader "Hidden/CompositeEffectsHDRP" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);

    TEXTURE2D(_SmokeTex);
    SAMPLER(sampler_SmokeTex);

    TEXTURE2D(_SmokeMaskTex);
    SAMPLER(sampler_SmokeMaskTex);

    TEXTURE2D(_CameraDepthTexture);
    SAMPLER(sampler_CameraDepthTexture);

    float4 _MainTex_TexelSize;
    int _DebugView;
    float _Sharpness;

    struct Attributes {
        float3 positionOS : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct Varyings {
        float2 uv : TEXCOORD0;
        float4 positionCS : SV_POSITION;
    };

    Varyings Vert(Attributes input) {
        Varyings output;
        output.positionCS = TransformObjectToHClip(input.positionOS);
        output.uv = input.uv;
        return output;
    }

    // Depth Texture Pass
    float4 DepthPass(Varyings input) : SV_Target {
        return SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv).rrrr;
    }

    // 9-Tap Catmull-Rom Filtering Pass
    float4 CatmullRomPass(Varyings input) : SV_Target {
        float2 samplePos = input.uv * _MainTex_TexelSize.zw;
        float2 texPos1 = floor(samplePos - 0.5f) + 0.5f;

        float2 f = samplePos - texPos1;

        float2 w0 = f * (-0.5f + f * (1.0f - 0.5f * f));
        float2 w1 = 1.0f + f * f * (-2.5f + 1.5f * f);
        float2 w2 = f * (0.5f + f * (2.0f - 1.5f * f));
        float2 w3 = f * f * (-0.5f + 0.5f * f);

        float2 w12 = w1 + w2;
        float2 offset12 = w2 / (w1 + w2);

        float2 texPos0 = texPos1 - 1;
        float2 texPos3 = texPos1 + 2;
        float2 texPos12 = texPos1 + offset12;

        texPos0 /= _MainTex_TexelSize.zw;
        texPos3 /= _MainTex_TexelSize.zw;
        texPos12 /= _MainTex_TexelSize.zw;

        float4 result = 0.0f;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos0.x, texPos0.y)) * w0.x * w0.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos12.x, texPos0.y)) * w12.x * w0.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos3.x, texPos0.y)) * w3.x * w0.y;

        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos0.x, texPos12.y)) * w0.x * w12.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos12.x, texPos12.y)) * w12.x * w12.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos3.x, texPos12.y)) * w3.x * w12.y;

        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos0.x, texPos3.y)) * w0.x * w3.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos12.x, texPos3.y)) * w12.x * w3.y;
        result += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(texPos3.x, texPos3.y)) * w3.x * w3.y;

        return result;
    }

    // Smoke Rendering Pass
    float4 SmokePass(Varyings input) : SV_Target {
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
        float4 smokeAlbedo = SAMPLE_TEXTURE2D(_SmokeTex, sampler_SmokeTex, input.uv);
        float smokeMask = saturate(SAMPLE_TEXTURE2D(_SmokeMaskTex, sampler_SmokeMaskTex, input.uv).r);

        // Apply sharpness filter
        float neighbor = _Sharpness * -1;
        float center = _Sharpness * 4 + 1;

        float4 n = SAMPLE_TEXTURE2D(_SmokeTex, sampler_SmokeTex, input.uv + _MainTex_TexelSize.xy * float2(0, 1));
        float4 e = SAMPLE_TEXTURE2D(_SmokeTex, sampler_SmokeTex, input.uv + _MainTex_TexelSize.xy * float2(1, 0));
        float4 s = SAMPLE_TEXTURE2D(_SmokeTex, sampler_SmokeTex, input.uv + _MainTex_TexelSize.xy * float2(0, -1));
        float4 w = SAMPLE_TEXTURE2D(_SmokeTex, sampler_SmokeTex, input.uv + _MainTex_TexelSize.xy * float2(-1, 0));

        float4 sharpenedSmoke = n * neighbor + e * neighbor + smokeAlbedo * center + s * neighbor + w * neighbor;

        return lerp(col, saturate(sharpenedSmoke), 1 - smokeMask);
    }
    ENDHLSLINCLUDE

    SubShader {
        Tags { "RenderPipeline" = "HDRenderPipeline" }

        Pass {
            Name "DepthPass"
            Tags { "LightMode" = "ForwardOnly" }
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment DepthPass
            ENDHLSL
        }

        Pass {
            Name "CatmullRomPass"
            Tags { "LightMode" = "ForwardOnly" }
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment CatmullRomPass
            ENDHLSL
        }

        Pass {
            Name "SmokePass"
            Tags { "LightMode" = "ForwardOnly" }
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment SmokePass
            ENDHLSL
        }
    }
}
