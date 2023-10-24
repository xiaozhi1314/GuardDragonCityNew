// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Effects/NiuQu & Mask" {
Properties {

    [Header(RenderingMobe)]
    [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend",int) = 0
    [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend",int) = 0  
    [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull",int) = 0

    [Header(Color)]

    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _AlphaScale("Alpha Scale", Range(0.0, 1.0)) = 1

    _XSpeed ("XSpeed", Float ) = 0
    _YSpeed ("YSpeed", Float ) = 0

    [Header(Mask)]

    _Mask_Tex ("Mask_Tex", 2D) = "white" {}
    _MXSpeed ("XSpeed", Float ) = 0
    _MYSpeed ("YSpeed", Float ) = 0

    [Header(NiuQu)]

    _NiuQu_Tex ("NiuQu_Tex", 2D) = "white" {}
    _NQXSpeed ("XSpeed", Float ) = 0
    _NQYSpeed ("YSpeed", Float ) = 0

    [Header(NiuQu Mask)]


    _NiuQu_Mask ("NiuQu_Mask", 2D) = "white" {}
    _NiuQu_QiangDu ("NiuQu_QiangDu", Float ) = 0

    _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

    Blend [_SrcBlend] [_DstBlend]  //自定义Blend混合方式。（srcFacxtor BlendOP DstFactor ） （源颜色*1 + 目标颜色*1）
    ColorMask RGB
    Cull [_Cull]
    Lighting Off
    ZWrite Off
    ZTest Off

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;
            float _XSpeed;
            float _YSpeed;
            float _MXSpeed;
            float _MYSpeed;
            float _NQXSpeed;
            float _NQYSpeed;
            sampler2D _NiuQu_Tex;
            float _NiuQu_QiangDu;
            sampler2D _Mask_Tex;
            sampler2D _NiuQu_Mask;
            half _AlphaScale;


            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;

                //
                float2 uv_NiuQu_Tex : TEXCOORD1;
                float2 uv_NiuQu_Mask : TEXCOORD2;
                float2 uv_Mask_Tex : TEXCOORD3;
                //

                //UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD4;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;
            float4 _NiuQu_Tex_ST;
            float4 _Mask_Tex_ST;
            float4 _NiuQu_Mask_ST;



            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;

                float2 uvN1 = v.texcoord + float2(_XSpeed*_Time.y,_Time.y*_YSpeed);//计算偏移速度的新UV

                o.texcoord = TRANSFORM_TEX(uvN1,_MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);//

                float2 uvN2 = v.texcoord + float2(_NQXSpeed*_Time.y,_Time.y*_NQYSpeed);//计算偏移速度的新UV
                o.uv_NiuQu_Tex = TRANSFORM_TEX(uvN2,_NiuQu_Tex);//计算UV进行Tiling与Offset变换后的新UV值
                o.uv_NiuQu_Mask = TRANSFORM_TEX(v.texcoord,_NiuQu_Mask);//计算UV进行Tiling与Offset变换后的新UV值   
                

                float2 uvN3 = v.texcoord + float2(_MXSpeed*_Time.y,_Time.y*_MYSpeed);//计算偏移速度的新UV
                o.uv_Mask_Tex = TRANSFORM_TEX(uvN3,_Mask_Tex);//计算UV进行Tiling与Offset变换后的新UV值



                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
                #endif

                float4 _NiuQu_Tex_var = tex2D(_NiuQu_Tex , i.uv_NiuQu_Tex);//二维纹理采样 NiuQu_Tex
                float4 _NiuQu_Mask_var = tex2D(_NiuQu_Mask , i.uv_NiuQu_Mask);//二维纹理采样 NiuQu_Mask
                float2 NiuQu_UV = i.texcoord + (float2(_NiuQu_Tex_var.r , _NiuQu_Tex_var.r) * _NiuQu_QiangDu * _NiuQu_Mask_var.r);//计算出扭曲的UV以方便MainTex后面使用
                float4 _MainTex_var = tex2D(_MainTex , NiuQu_UV);//使用扭曲的UV对 MainTex 二维纹理采样
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,i.uv_Mask_Tex);//二维纹理采样 Mask_Tex

                fixed3 colRGB = (i.color.rgb * _TintColor.rgb * _MainTex_var.rgb * 2.0);//通过各方面RGB的值相乘，计算出总的RGB值
                fixed4 col = fixed4(colRGB, (i.color.a * (_TintColor.a * 2.0) * _MainTex_var.a * _Mask_Tex_var.r));//计算出RGB值和Alpha值
                col.a = saturate(col.a * _AlphaScale); // alpha should not have double-brightness applied to it, but we can't fix that legacy behavior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)

               // UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
                return col;
            }
            ENDCG
        }
    }
}
}
