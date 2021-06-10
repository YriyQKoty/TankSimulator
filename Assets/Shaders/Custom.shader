Shader "Custom/RasterizerTestShader"
{
	SubShader
	{
	Tags { "RenderType" = "Opaque" }
	Pass
	{
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	struct appdata
	{
	float4 vertex : POSITION;
	float4 color4 : COLOR;
	};
	struct v2f
	{
	float4 vertex : SV_POSITION;
	float4 color4 : COLOR;
	};
	v2f vert(appdata v)
	{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.color4 = v.color4;
	return o;
	}
	float4 frag(v2f i) : SV_Target
	{
	return i.color4;
	}
	ENDCG
	}
	}
}