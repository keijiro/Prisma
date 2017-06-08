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

Shader "Kvant/SprayMV/Opaque"
{
    Properties
    {
        _PositionBuffer("", 2D) = "black"{}
        _RotationBuffer("", 2D) = "red"{}

        _MainTex("-", 2D) = "white"{}

        [KeywordEnum(Single, Animate, Random)]
        _ColorMode("", Float) = 0
        _Color("", Color) = (1, 1, 1, 1)
        _Color2("", Color) = (0.5, 0.5, 0.5, 1)

        _Metallic("", Range(0, 1)) = 0
        _Smoothness("", Range(0, 1)) = 0

        _NormalMap("", 2D) = "bump"{}
        _NormalScale("", Range(0, 2)) = 1

        _OcclusionMap("", 2D) = "white"{}
        _OcclusionStrength("", Range(0, 1)) = 1

        _Emission("", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Tags { "LightMode" = "MotionVectors" }
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "Motion.cginc"
            ENDCG
        }
        CGPROGRAM
        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma shader_feature _COLORMODE_RANDOM
        #pragma shader_feature _ALBEDOMAP
        #pragma shader_feature _NORMALMAP
        #pragma shader_feature _OCCLUSIONMAP
        #pragma shader_feature _EMISSION
        #pragma target 3.0
        #include "Opaque.cginc"
        ENDCG
    }
    CustomEditor "Kvant.SprayMVMaterialEditor"
}
