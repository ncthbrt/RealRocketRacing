Shader "Mask/MaskShader" {
	Properties {
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex ("Sprite (RGB)", 2D) = "white" {}
		_Mask ("Mask (RGB)", 2D) = "white" {}
	}
	SubShader{

		Tags { "RenderType" = "Transparent" "Queue" = "Transparent+2"}
		LOD 200
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _Mask;			

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				float mask = tex2D(_Mask, IN.texcoord).a;
				c.rgba *= mask*IN.color.a;				
				//c.a= mask*IN.color.a;
				return c;
			}			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
