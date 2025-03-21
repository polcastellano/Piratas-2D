Shader "Custom/URP_SpriteLitFadeLight"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        _Fade("Fade", Range(0, 1)) = 1 // Propiedad de Fade
        _EmissionColor("Emission Color", Color) = (0, 0, 0, 1) // Propiedad de Emisión
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

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
                float3 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float _Fade; // Control de Fade
                float4 _EmissionColor; // Color de Emisión
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

                // Aplica el valor de Fade al canal alfa
                texColor.a *= _Fade;

                // Añadimos el color de emisión al color de la textura
                texColor.rgb += _EmissionColor.rgb * _EmissionColor.a;

                return texColor;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
