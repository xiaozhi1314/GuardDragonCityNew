Shader "Effects/溶解_水浪"
{
	Properties
	{
		_MainTex("主纹理", 2D) = "white" {}
		_XSpeed("X Speed", Float) = -1
		_YSpeed("Y Speed", Float) = 0
		_MaskTex("遮罩纹理", 2D) = "white" {}
		_DissolveTex("溶解纹理", 2D) = "white" {}
		_Dissolve_Speed_X("溶解速度X", Float) = -1
		_Dissolve_Speed_Y("溶解速度Y", Float) = 0
		_DissolveRange("溶解范围", Float) = 0
		_SoftDissolve("溶解硬度", Range( 0 , 1)) = 1
		_DissolveIntensity("溶解强度", Range( 0 , 1.05)) = 0.5435294
		_NoiseTex("扭曲纹理", 2D) = "white" {}
		_Noise_Speed_U("扭曲速度U", Float) = 0
		_Noise_Speed_V("扭曲速度V", Float) = 0
		_NoiseIntensity("扭曲强度", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float _XSpeed;
			uniform float _YSpeed;
			uniform float4 _MainTex_ST;
			uniform sampler2D _NoiseTex;
			uniform float4 _NoiseTex_ST;
			uniform float _Noise_Speed_U;
			uniform float _Noise_Speed_V;
			uniform float _NoiseIntensity;
			uniform sampler2D _MaskTex;
			uniform float4 _MaskTex_ST;
			uniform float _SoftDissolve;
			uniform sampler2D _DissolveTex;
			uniform float _Dissolve_Speed_X;
			uniform float _Dissolve_Speed_Y;
			uniform float4 _DissolveTex_ST;
			uniform float _DissolveRange;
			uniform float _DissolveIntensity;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 appendResult11 = (float2(_XSpeed , _YSpeed));
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 panner12 = ( 1.0 * _Time.y * appendResult11 + uv_MainTex);
				float2 uv_NoiseTex = i.ase_texcoord1.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 appendResult73 = (float2(( _Noise_Speed_U * _Time.y ) , ( _Time.y * _Noise_Speed_V )));
				float4 tex2DNode75 = tex2D( _NoiseTex, ( uv_NoiseTex + appendResult73 ) );
				float2 appendResult60 = (float2(tex2DNode75.r , tex2DNode75.r));
				float2 temp_output_61_0 = ( appendResult60 * _NoiseIntensity );
				float4 tex2DNode1 = tex2D( _MainTex, ( panner12 + temp_output_61_0 ) );
				float2 uv_MaskTex = i.ase_texcoord1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float4 _Vector0 = float4(0,1,0.5,1);
				float temp_output_100_0 = (_Vector0.z + (_SoftDissolve - _Vector0.x) * (_Vector0.w - _Vector0.z) / (_Vector0.y - _Vector0.x));
				float2 appendResult17 = (float2(_Dissolve_Speed_X , _Dissolve_Speed_Y));
				float2 uv_DissolveTex = i.ase_texcoord1.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 panner18 = ( 1.0 * _Time.y * appendResult17 + uv_DissolveTex);
				float2 texCoord35 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult43 = clamp( ( ( tex2D( _DissolveTex, ( temp_output_61_0 + panner18 ) ).r + ( texCoord35.y * 0.1 ) ) * texCoord35.y * _DissolveRange ) , 0.0 , 1.0 );
				float smoothstepResult105 = smoothstep( ( 1.0 - temp_output_100_0 ) , temp_output_100_0 , ( clampResult43 + 1.0 + ( _DissolveIntensity * -2.0 ) ));
				float4 appendResult5 = (float4((tex2DNode1).rgb , ( tex2DNode1.a * tex2D( _MaskTex, uv_MaskTex ).r * smoothstepResult105 )));
				
				
				finalColor = ( appendResult5 * i.ase_color );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}