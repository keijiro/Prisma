Shader "Hidden/Screen Blit"
{
    Properties
    {
        _MainTex("", 2D) = "white" {}
        _Color("", Color) = (1, 1, 1, 1)
        _NoiseTex("", 2D) = "black" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    sampler2D _NoiseTex;
    float4 _NoiseTex_TexelSize;

    half4 _Color;
    float2 _Scale;
    float2 _Offset;
    half _DitherAmount;

    half UVMask(float2 uv)
    {
        return saturate(1 - dot(abs(floor(uv)), 1));
    }

    half4 frag_blit(v2f_img i) : SV_Target
    {
        float2 uv = i.uv * _Scale + _Offset;
        half3 src = tex2D(_MainTex, uv).rgb * UVMask(uv) * _Color;

        return half4(src, 1);
    }

    half4 frag_blit_dither(v2f_img i) : SV_Target
    {
        float2 uv = i.uv * _Scale + _Offset;
        half3 src = tex2D(_MainTex, uv).rgb * UVMask(uv) * _Color;

        uv = i.uv * _ScreenParams.xy * _NoiseTex_TexelSize.xy;
        float dither = tex2D(_NoiseTex, uv).a;

        dither = mad(dither, 2, -1);
        dither = sign(dither) * (1 - sqrt(1 - abs(dither)));
        dither *= _DitherAmount / 255;

        src = GammaToLinearSpace(LinearToGammaSpace(src) + dither);

        return half4(src, 1);
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
            #pragma fragment frag_blit_dither
            ENDCG
        }
    }
}
