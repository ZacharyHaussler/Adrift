Shader "Custom/URP_MeshEdgeGlow"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _MetallicMap ("Metallic Map", 2D) = "black" {}
        _HeightMap ("Height Map", 2D) = "black" {}

        _BaseColor ("Base Color", Color) = (1,1,1,1)

        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _EdgeWidth ("Edge Width", Float) = 0.02
        _EdgeEmission ("Edge Emission", Float) = 4

        _Tile ("Tiling", Float) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                float3 bary       : TEXCOORD1; // Barycentric coords required
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float2 uv          : TEXCOORD1;
                float3 bary        : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            TEXTURE2D(_MetallicMap); SAMPLER(sampler_MetallicMap);
            TEXTURE2D(_HeightMap); SAMPLER(sampler_HeightMap);

            float4 _BaseColor;
            float4 _EdgeColor;
            float _EdgeWidth;
            float _EdgeEmission;
            float _Tile;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv * _Tile;
                OUT.bary = IN.bary;

                return OUT;
            }

            float EdgeFactor(float3 bary, float width)
            {
                float3 d = fwidth(bary);
                float3 a3 = smoothstep(d * width, 0, bary);
                return 1 - min(min(a3.x, a3.y), a3.z);
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float3 normalWS = normalize(IN.normalWS);

                float4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                baseCol *= _BaseColor;

                float metallic = SAMPLE_TEXTURE2D(_MetallicMap, sampler_MetallicMap, IN.uv).r;

                float height = SAMPLE_TEXTURE2D(_HeightMap, sampler_HeightMap, IN.uv).r;
                baseCol.rgb += height * 0.05;

                float edge = EdgeFactor(IN.bary, _EdgeWidth);

                float3 finalColor = lerp(baseCol.rgb, _EdgeColor.rgb, edge);
                float3 emission = edge * _EdgeColor.rgb * _EdgeEmission;

                return float4(finalColor + emission, 1);
            }

            ENDHLSL
        }
    }
}