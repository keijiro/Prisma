Shader "Hidden/Screen Blit"
{
    Properties
    {
        _MainTex("", 2D) = "white" {}
        _Color("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;
    half4 _Color;
    float2 _Scale;
    float2 _Offset;

    half UVMask(float2 uv)
    {
        return saturate(1 - dot(abs(floor(uv)), 1));
    }

    half4 frag_blit(v2f_img i) : SV_Target
    {
        float2 uv = i.uv * _Scale + _Offset;
        return tex2D(_MainTex, uv) * UVMask(uv) * _Color;
    }

    half4 frag_test(v2f_img i) : SV_Target
    {
        float2 uv = i.uv * _Scale + _Offset;
        float mask = UVMask(uv);

        float2 norm = float2(1, _MainTex_TexelSize.x * _MainTex_TexelSize.w);
        float2 grid_uv = abs(0.5 - frac(uv * norm * 20));
        grid_uv = (grid_uv - 0.48) * _ScreenParams.x / 20;
        float grid = max(grid_uv.x, grid_uv.y);

        float2 circ_uv = (uv - 0.5) * 5 * norm;
        float c = GammaToLinearSpace(circ_uv.x / 2 + 0.5);
        c = lerp(c, 0.5, abs(circ_uv.y) > 0.2);
        c = lerp(c, grid, length(circ_uv) > 1);

        return half4(lerp(half3(1, 0, 0), c, mask), 1);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_blit
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_test
            ENDCG
        }
    }
}
