Shader "Prisma/Emission By Vertex Color"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard
        #pragma target 3.0

        struct Input
        {
            half4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = 1;
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Emission = IN.color;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
