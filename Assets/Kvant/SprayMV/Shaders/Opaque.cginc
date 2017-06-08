//
// Kvant/SprayMV - Particle system with motion vectors support
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#include "Common.cginc"

sampler2D _MainTex;

half _Metallic;
half _Smoothness;

sampler2D _NormalMap;
half _NormalScale;

sampler2D _OcclusionMap;
half _OcclusionStrength;

half _Emission;

struct Input
{
    float2 uv_MainTex;
    fixed4 color : COLOR;
};

void vert(inout appdata_full v)
{
    float4 uv = float4(v.texcoord1.xy, 0, 0);

    float4 p = tex2Dlod(_PositionBuffer, uv);
    float4 r = tex2Dlod(_RotationBuffer, uv);

    float l = p.w + 0.5;
    float s = ScaleAnimation(uv, l);

    v.vertex.xyz = RotateVector(v.vertex.xyz, r) * s + p.xyz;
    v.normal = RotateVector(v.normal, r);
#if _NORMALMAP
    v.tangent.xyz = RotateVector(v.tangent.xyz, r);
#endif
    v.color = ColorAnimation(uv, l);
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
#if _ALBEDOMAP
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = IN.color.rgb * c.rgb;
#else
    o.Albedo = IN.color.rgb;
#endif

#if _NORMALMAP
    fixed4 n = tex2D(_NormalMap, IN.uv_MainTex);
    o.Normal = UnpackScaleNormal(n, _NormalScale);
#endif

#if _OCCLUSIONMAP
    fixed occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
    o.Occlusion = LerpOneTo(occ, _OcclusionStrength);
#endif

#if _EMISSION
    o.Emission = o.Albedo * _Emission;
#endif

    o.Metallic = _Metallic;
    o.Smoothness = _Smoothness;
}
