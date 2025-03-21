Shader "Custom/URP_SpriteLitEnemigo"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        _Fade("Fade", Range(0, 1)) = 1 // Nueva propiedad Fade
        _Color("Color", Color) = (1,1,1,1) // Nueva propiedad Color
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float _Fade; // Valor de Fade
                float4 _Color; // Añadimos el valor de Color
            CBUFFER_END

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Aplica el color multiplicándolo por la textura
                texColor.rgb *= _Color.rgb;

                // Aplica el valor de Fade al canal alfa
                texColor.a *= _Fade * _Color.a;

                return texColor;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
