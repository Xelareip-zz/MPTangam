Shader "Edges/EdgesParticles"
{
	Properties
	{
		_MainTex("Base (RGBA)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha, One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				half4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				half4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= i.color.rgb;
				col.r = 1;
				col.gb = 0;
				return col;
			}
			ENDCG
		}
	}
}
