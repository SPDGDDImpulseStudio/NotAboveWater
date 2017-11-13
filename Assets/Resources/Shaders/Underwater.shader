// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Underwater"
{
	Properties
	{
		_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Distortion ("Distortion Texture", 2D) = "bump" {}
		_DistAmt ("Distortion Amount", Range(0,64)) = 10.0
		_DistVect ("Distortion Tiling", Vector) = (0,0,1,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos (v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 _Tint;
			sampler2D _MainTex;
			sampler2D _Distortion;
			float4 _MainTex_TexelSize;
			half _DistAmt;
			float4 _DistVect;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 b = tex2D (_Distortion, i.uv * _DistVect.zw + _DistVect.xy);
				float2 offset = UnpackNormal (b) * _DistAmt * _MainTex_TexelSize;
				i.uv.xy = offset + i.uv.xy;
				fixed4 col = tex2D (_MainTex, i.uv) * _Tint;
				return col;
			}
			ENDCG
		}
	}
}