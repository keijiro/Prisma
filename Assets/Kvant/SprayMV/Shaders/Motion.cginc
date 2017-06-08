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

float4x4 _NonJitteredVP;
float4x4 _PreviousVP;
float4x4 _PreviousM;
float _MotionScale;

struct appdata
{
    float4 vertex : POSITION;
    float2 texcoord1 : TEXCOORD1;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float4 transfer0 : TEXCOORD0;
    float4 transfer1 : TEXCOORD1;
};

v2f vert(appdata v)
{
    float4 uv = float4(v.texcoord1.xy, 0, 0);

    // Position, Rotation, Scale
    float4 p0 = tex2Dlod(_PreviousPositionBuffer, uv);
    float4 p1 = tex2Dlod(_PositionBuffer, uv);
    float4 r0 = tex2Dlod(_PreviousRotationBuffer, uv);
    float4 r1 = tex2Dlod(_RotationBuffer, uv);
    float s0 = ScaleAnimation(uv, p0.w + 0.5);
    float s1 = ScaleAnimation(uv, p1.w + 0.5);

    // Apply the local transformation.
    float3 vp0 = RotateVector(v.vertex.xyz, r0) * s0 + p0.xyz;
    float3 vp1 = RotateVector(v.vertex.xyz, r1) * s1 + p1.xyz;

    // Transfer the data to the pixel shader.
    v2f o;
    o.vertex = UnityObjectToClipPos(float4(vp1, 1));
    o.transfer0 = mul(_PreviousVP, mul(_PreviousM,  float4(vp0, 1)));
    o.transfer1 = mul(_NonJitteredVP, mul(unity_ObjectToWorld, float4(vp1, 1)));
    return o;
}

half4 frag(v2f i) : SV_Target
{
    float3 hp0 = i.transfer0.xyz / i.transfer0.w;
    float3 hp1 = i.transfer1.xyz / i.transfer1.w;

    float2 vp0 = (hp0.xy + 1) / 2;
    float2 vp1 = (hp1.xy + 1) / 2;

#if UNITY_UV_STARTS_AT_TOP
    vp0.y = 1 - vp0.y;
    vp1.y = 1 - vp1.y;
#endif

    return half4(vp1 - vp0, 0, 1);
}
