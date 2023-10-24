// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Effects/UV Speed Mask" {
	Properties{

		[Header(RenderingMobe)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend",int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend",int) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull",int) = 0
		[Header(Color)]

		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_Main_XSpeed("Main_XSpeed", Float) = 0
		_Main_YSpeed("Main_YSpeed", Float) = 0

		_MaskTex1("Mask Tex1",2D) = "white"{}
		_Mask1_Speed_X("Mask1_Speed_X", Float) = 0
		_Mask1_Speed_Y("Mask1_Speed_Y", Float) = 0

		_MaskTex2("Mask Tex2",2D) = "white"{}
		_Mask2_Speed_X("Mask2_Speed_X", Float) = 0
		_Mask2_Speed_Y("Mask2_Speed_Y", Float) = 0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

	}

		Category{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}
			Blend[_SrcBlend][_DstBlend]  //自定义Blend混合方式。（srcFacxtor BlendOP DstFactor ） （源颜色*1 + 目标颜色*1）
			ColorMask RGB
			Cull[_Cull]
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

					#include "UnityCG.cginc"
					#include "UnityUI.cginc"

					fixed4 _TintColor;
					fixed4 _Color;
					float4 _ClipRect;
					sampler2D _MainTex;
					float4 _MainTex_ST;
					float _Main_XSpeed;
					float _Main_YSpeed;

					sampler2D _MaskTex1;
					float4 _MaskTex1_ST;
					float _Mask1_Speed_X;
					float _Mask1_Speed_Y;

					sampler2D _MaskTex2;
					float4 _MaskTex2_ST;
					float _Mask2_Speed_X;
					float _Mask2_Speed_Y;

					struct appdata_t {
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						fixed4 color : COLOR;
						float2 Main_UV : TEXCOORD0;
						float2 mask1_uv : TEXCOORD1;
						float2 mask2_uv : TEXCOORD2;
						half2 texcoord  : TEXCOORD3;
						float4 worldPosition : TEXCOORD4;
					};

					v2f vert(appdata_t v)
					{
						v2f o;
						o.worldPosition = v.vertex;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.color = v.color;

						fixed xV = _Main_XSpeed * _Time.y;
						fixed yV = _Main_YSpeed * _Time.y;
						o.Main_UV = TRANSFORM_TEX(v.texcoord,_MainTex) + fixed2(xV, yV);

						fixed mask1_xV = _Mask1_Speed_X * _Time.y;
						fixed mask1_yV = _Mask1_Speed_Y * _Time.y;
						o.mask1_uv = TRANSFORM_TEX(v.texcoord, _MaskTex1) + fixed2(mask1_xV, mask1_yV);

						fixed mask2_xV = _Mask2_Speed_X * _Time.y;
						fixed mask2_yV = _Mask2_Speed_Y * _Time.y;
						o.mask2_uv = TRANSFORM_TEX(v.texcoord, _MaskTex2) + fixed2(mask2_xV, mask2_yV);


						return o;
					}


					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 _MainTex_var = tex2D(_MainTex, i.Main_UV);

						fixed4 _MaskTex1_var = tex2D(_MaskTex1, i.mask1_uv);
						//fixed4 _MaskTex1_var = tex2D(_MaskTex1,TRANSFORM_TEX(mask1_uv, _MaskTex1));

						fixed4 _MaskTex2_var = tex2D(_MaskTex2, i.mask2_uv);

						fixed3 col = 2.0 * i.color.xyz * _TintColor.xyz * _MainTex_var.xyz;
						fixed col_alpha = saturate(2.0 * i.color.a * _TintColor.a * _MainTex_var.a * _MaskTex1_var.a * _MaskTex2_var.a);
						fixed4 finalcol = fixed4(col, col_alpha);

						finalcol.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
						finalcol.rgb *= finalcol.a;
#ifdef UNITY_UI_ALPHACLIP
						clip(finalcol.a - 0.01);
#endif

						return finalcol;
					}
					ENDCG
				}
			}
		}
}
