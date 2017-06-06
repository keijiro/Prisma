// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

Shader "Cloner/Surface"
{
    Properties
    {
        _MainTex("Albedo Map", 2D) = "white" {}
        _Color("Albedo Color", Color) = (1, 1, 1, 1)
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _Metallic("Metallic", Range(0, 1)) = 0
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalScale("Normal Scale", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert addshadow
        #pragma instancing_options procedural:setup
        #pragma target 4.0

        struct Input
        {
            float2 uv_MainTex;
            float GradientParam : COLOR;
        };

        sampler2D _MainTex;
        half4 _Color;
        half _Smoothness;
        half _Metallic;

        sampler2D _NormalMap;
        half _NormalScale;

        int _RandomSeed;

        half3 _GradientA;
        half3 _GradientB;
        half3 _GradientC;
        half3 _GradientD;

        float4x4 _LocalToWorld;
        float4x4 _WorldToLocal;

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        StructuredBuffer<float4> _TransformBuffer;
        uint _InstanceCount;
        #endif

        float Random(float u, float v)
        {
            v += _RandomSeed * 30.4953;
            float f = dot(float2(12.9898, 78.233), float2(u, v));
            return frac(43758.5453 * sin(f));
        }

        half3 CosineGradient(half param)
        {
            half3 c = _GradientB * cos(_GradientC * param + _GradientD);
            return GammaToLinearSpace(saturate(c + _GradientA));
        }

        void setup()
        {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

            uint id = unity_InstanceID;

            // Retrieve a transformation from TransformBuffer.
            float4 ps = _TransformBuffer[id + _InstanceCount * 0];
            float3 bx = _TransformBuffer[id + _InstanceCount * 1];
            float3 by = _TransformBuffer[id + _InstanceCount * 2];
            float3 bz = cross(bx, by);

            // Object to world matrix.
            float3 v1 = bx * ps.w;
            float3 v2 = by * ps.w;
            float3 v3 = bz * ps.w;

            float4x4 o2w = float4x4(
                v1.x, v2.x, v3.x, ps.x,
                v1.y, v2.y, v3.y, ps.y,
                v1.z, v2.z, v3.z, ps.z,
                0, 0, 0, 1
            );

            // World to object matrix.
            float3 v4 = bx / ps.w;
            float3 v5 = by / ps.w;
            float3 v6 = bz / ps.w;

            float4x4 w2o = float4x4(
                v1.x, v1.y, v1.z, -ps.x,
                v2.x, v2.y, v2.z, -ps.x,
                v3.x, v3.y, v3.z, -ps.x,
                0, 0, 0, 1
            );

            unity_ObjectToWorld = mul(_LocalToWorld, o2w);
            unity_WorldToObject = mul(w2o, _WorldToLocal);

            #endif
        }

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

            uint id = unity_InstanceID;
            v.color = 0.5 + _TransformBuffer[id + _InstanceCount * 2].w;

            #endif
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

            uint id = unity_InstanceID;
            uv.x += Random(id * 0.0001, 0.2741);
            uv.y += Random(0.3179, id * 0.0001);

            #endif

            half3 c = tex2D(_MainTex, uv).rgb * _Color.rgb;
            c *= CosineGradient(IN.GradientParam);

            o.Albedo = c;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;

            o.Normal = UnpackScaleNormal(tex2D(_NormalMap, uv), _NormalScale);
        }

        ENDCG
    }
    CustomEditor "Cloner.ClonerMaterialEditor"
}
