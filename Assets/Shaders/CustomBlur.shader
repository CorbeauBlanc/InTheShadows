Shader "Custom/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Range(0, .1)) = 0
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

			#define SAMPLES 30

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _BlurSize;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = 0;

				/* for(float iY = 0; iY < SAMPLES; iY++)
					for(float iX = 0; iX < SAMPLES; iX++)
						col += tex2D(_MainTex, i.uv + float2((iX / (SAMPLES - 1) - .5) * _BlurSize, (iY / (SAMPLES - 1) - .5) * _BlurSize));

				col = col / (SAMPLES * SAMPLES);
 */
				for(float iY = 0; iY < SAMPLES; iY++)
					col += tex2D(_MainTex, i.uv + float2(0, (iY / (SAMPLES - 1) - .5) * _BlurSize));

				col = col / SAMPLES;
				return col;
			}
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define SAMPLES 30

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _BlurSize;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = 0;

				for(float iX = 0; iX < SAMPLES; iX++)
					col += tex2D(_MainTex, i.uv + float2((iX / (SAMPLES - 1) - .5) * _BlurSize, 0));

				col = col / SAMPLES;

				return col;
			}
			ENDCG
		}
	}
}
