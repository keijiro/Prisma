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

Shader "Hidden/Kvant/SprayMV/Kernels"
{
    Properties
    {
        _PositionBuffer("", 2D) = ""{}
        _VelocityBuffer("", 2D) = ""{}
        _RotationBuffer("", 2D) = ""{}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma vertex vert_img
            #pragma fragment frag_InitPosition
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma vertex vert_img
            #pragma fragment frag_InitVelocity
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma target 3.0
            #pragma vertex vert_img
            #pragma fragment frag_InitRotation
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma vertex vert_img
            #pragma fragment frag_UpdatePosition
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma vertex vert_img
            #pragma fragment frag_UpdateVelocity
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #include "Kernels.cginc"
            #pragma vertex vert_img
            #pragma fragment frag_UpdateRotation
            #pragma target 3.0
            ENDCG
        }
    }
}
