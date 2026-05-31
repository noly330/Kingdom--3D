Shader "UI/RingGlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HDR] _GlowColor ("Glow Color", Color) = (0.2,0.8,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 1.5
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
        _PulseStrength ("Pulse Strength", Range(0, 1)) = 0.25
        _Alpha ("Alpha", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _PulseSpeed;
            float _PulseStrength;
            float _Alpha;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseColor = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
                float pulse = 1.0 + sin(_Time.y * _PulseSpeed) * _PulseStrength;
                fixed4 glow = _GlowColor * _GlowIntensity * pulse;

                fixed4 col;
                col.rgb = baseColor.rgb + glow.rgb * baseColor.a;
                col.a = baseColor.a * _Alpha;
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                return col;
            }
            ENDCG
        }
    }
}
