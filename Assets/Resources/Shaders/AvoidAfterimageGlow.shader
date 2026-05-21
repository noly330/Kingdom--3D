Shader "Kingdom/Avoid Afterimage Glow"
{
    Properties
    {
        [HDR] _BaseColor("Base Color", Color) = (0.05, 0.55, 1.0, 0.28)
        [HDR] _RimColor("Rim Color", Color) = (0.15, 0.85, 1.0, 1.0)
        _Alpha("Alpha", Range(0, 1)) = 0.32
        _FresnelPower("Fresnel Power", Range(0.5, 8)) = 2.2
        _EmissionIntensity("Emission Intensity", Range(0, 8)) = 2.6
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "AvoidAfterimageGlow"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha One
            ZWrite Off
            ZTest LEqual
            Cull Back
            Lighting Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _RimColor;
                half _Alpha;
                half _FresnelPower;
                half _EmissionIntensity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);

                output.positionCS = positionInputs.positionCS;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 normalWS = normalize(input.normalWS);
                half3 viewDirWS = normalize(input.viewDirWS);
                half fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _FresnelPower);

                half3 color = (_BaseColor.rgb + _RimColor.rgb * fresnel) * _EmissionIntensity;
                half alpha = saturate(_Alpha + fresnel * _RimColor.a * 0.55);
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
