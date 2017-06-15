// Shadow Slicer shader: See ShadowSlicer.cs for details.

Shader "Hidden/Prisma/ShadowSlicer"
{
    Properties
    {
        _Color("", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        ZTest always ZWrite off

        // Forward base: draws black full-screen quad.
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "UnityCG.cginc"

            float4 Vert(float4 position : POSITION) : SV_POSITION
            {
                return float4(position.xy * float2(2, -2), 0, 1);
            }

            fixed4 Frag() : SV_Target
            {
                return 0;
            }

            ENDCG
        }

        // Forward add:
        // renders a slice of a shadow volume with a full-screen quad.
        Pass
        {
            Tags { "LightMode" = "ForwardAdd" }

            Blend One One

            CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile_fwdadd_fullshadows
            #pragma skip_variants DIRECTIONAL SHADOWS_SCREEN POINT_COOKIE DIRECTIONAL_COOKIE

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            fixed4 _Color;

            struct Attributes
            {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float3 worldPosition : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            Varyings Vert(Attributes input)
            {
                Varyings o;
                o.position = float4(input.position.xy * float2(2, -2), 0, 1);
                o.worldPosition = mul(unity_ObjectToWorld, input.position).xyz;
                o.worldNormal = UnityObjectToWorldNormal(input.normal);
                return o;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float3 lightDir = UnityWorldSpaceLightDir(input.worldPosition);
                UNITY_LIGHT_ATTENUATION(atten, input, input.worldPosition)
                atten *= dot(normalize(lightDir), input.worldNormal);
                return half4(_Color.rgb * _LightColor0.rgb * atten, 1);
            }

            ENDCG
        }
    }
}
