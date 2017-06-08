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
#include "SimplexNoiseGrad3D.cginc"

float3 _EmitterPos;
float3 _EmitterSize;
float2 _LifeParams;   // 1/min, 1/max
float4 _Direction;    // x, y, z, spread
float2 _SpeedParams;  // speed, randomness
float4 _Acceleration; // x, y, z, drag
float3 _SpinParams;   // spin*2, speed-to-spin*2, randomness
float2 _NoiseParams;  // freq, amp
float3 _NoiseOffset;
float4 _Config;       // throttle, dT, time

// Particle generator functions
float4 NewParticlePosition(float2 uv)
{
    float t = _Config.z;

    // Random position
    float3 p = float3(UVRandom(uv, t), UVRandom(uv, t + 1), UVRandom(uv, t + 2));
    p = (p - 0.5) * _EmitterSize + _EmitterPos;

    // Throttling: discards particle emission by adding offset.
    float4 offs = float4(1e8, 1e8, 1e8, -1) * (uv.x > _Config.x);

    return float4(p, 0.5) + offs;
}

float4 NewParticleVelocity(float2 uv)
{
    // Random vector
    float3 v = float3(UVRandom(uv, 3), UVRandom(uv, 4), UVRandom(uv, 5));
    v = (v - 0.5) * 2;

    // Spreading
    v = lerp(_Direction.xyz, v, _Direction.w);

    // Speed
    v = normalize(v) * _SpeedParams.x;
    v *= 1.0 - UVRandom(uv, 6) * _SpeedParams.y;

    return float4(v, 0);
}

float4 NewParticleRotation(float2 uv)
{
    // Uniform random unit quaternion
    // http://www.realtimerendering.com/resources/GraphicsGems/gemsiii/urot.c
    float r = UVRandom(uv, 7);
    float r1 = sqrt(1 - r);
    float r2 = sqrt(r);
    float t1 = UNITY_PI * 2 * UVRandom(uv, 8);
    float t2 = UNITY_PI * 2 * UVRandom(uv, 9);
    return float4(sin(t1) * r1, cos(t1) * r1, sin(t2) * r2, cos(t2) * r2);
}

// Deterministic random rotation axis
float3 RotationAxis(float2 uv)
{
    // Uniformaly distributed points
    // http://mathworld.wolfram.com/SpherePointPicking.html
    float u = UVRandom(uv, 10) * 2 - 1;
    float u2 = sqrt(1 - u * u);
    float sn, cs;
    sincos(UVRandom(uv, 11) * UNITY_PI * 2, sn, cs);
    return float3(u2 * cs, u2 * sn, u);
}

// Pass 0: initial position
float4 frag_InitPosition(v2f_img i) : SV_Target
{
    // Crate a new particle and randomize its initial life.
    float4 p = NewParticlePosition(i.uv);
    p.w -= UVRandom(i.uv, 14);
    return p;
}

// Pass 1: initial velocity
float4 frag_InitVelocity(v2f_img i) : SV_Target
{
    return NewParticleVelocity(i.uv);
}

// Pass 2: initial rotation
float4 frag_InitRotation(v2f_img i) : SV_Target
{
    return NewParticleRotation(i.uv);
}

// Pass 3: position update
float4 frag_UpdatePosition(v2f_img i) : SV_Target
{
    float4 p = tex2D(_PositionBuffer, i.uv);
    float3 v = tex2D(_VelocityBuffer, i.uv).xyz;

    // Decaying
    float dt = _Config.y;
    p.w -= lerp(_LifeParams.x, _LifeParams.y, UVRandom(i.uv, 12)) * dt;

    if (p.w > -0.5)
    {
        // Applying the velocity
        p.xyz += v * dt;
        return p;
    }
    else
    {
        // Respawn
        return NewParticlePosition(i.uv);
    }
}

// Pass 4: velocity update
float4 frag_UpdateVelocity(v2f_img i) : SV_Target
{
    float4 p = tex2D(_PositionBuffer, i.uv);
    float3 v = tex2D(_VelocityBuffer, i.uv).xyz;

    if (p.w < 0.5)
    {
        // Drag
        v *= _Acceleration.w; // dt is pre-applied in script

        // Constant acceleration
        float dt = _Config.y;
        v += _Acceleration.xyz * dt;

        // Acceleration by turbulent noise
        float3 np = (p.xyz + _NoiseOffset) * _NoiseParams.x;
        float3 n1 = snoise_grad(np);
        float3 n2 = snoise_grad(np + float3(21.83, 13.28, 7.32));
        v += cross(n1, n2) * _NoiseParams.y * dt;

        return float4(v, 0);
    }
    else
    {
        // Respawn
        return NewParticleVelocity(i.uv);
    }
}

// Pass 5: rotation update
float4 frag_UpdateRotation(v2f_img i) : SV_Target
{
    float4 r = tex2D(_RotationBuffer, i.uv);
    float3 v = tex2D(_VelocityBuffer, i.uv).xyz;

    // Delta angle
    float dt = _Config.y;
    float theta = (_SpinParams.x + length(v) * _SpinParams.y) * dt;

    // Randomness
    theta *= 1.0 - UVRandom(i.uv, 13) * _SpinParams.z;

    // Spin quaternion
    float sn, cs;
    sincos(theta, sn, cs);
    float4 dq = float4(RotationAxis(i.uv) * sn, cs);

    // Applying the quaternion and normalize the result.
    return normalize(QMult(dq, r));
}
