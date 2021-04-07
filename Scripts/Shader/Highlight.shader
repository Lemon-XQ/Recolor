// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Highlight"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}									//主纹理
		_EdgeAlphaThreshold("Edge Alpha Threshold", Float) = 0				//边界透明度和的阈值
		_EdgeColor("Edge Color", Color) = (0,1,0,1)									//边界颜色
		_EdgeDampRate("Edge Damp Rate", Float) = 2									//边缘渐变的分母
		_OriginAlphaThreshold("OriginAlphaThreshold", range(0.1, 1)) = 0.2			//原始颜色透明度剔除的阈值
		_EdgeRadius("EdgeRadius",Range(0,30)) = 20
		[Toggle(_ShowOutline)] _ShowOutline("Show Outline", Int) = 0					//Toggle开关来控制是否显示边缘
		[Toggle(_Painted)] _Painted("Painted", Int) = 0					//Toggle开关来控制是否显示边缘
		_Color("Color",Color)=(0,0,0,1)
		_PreColor("PreColor",Color)=(0,0,0,1)
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Ztest Always Cull Off ZWrite Off
			CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		//#pragma shader_feature _ShowOutline
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		fixed _EdgeAlphaThreshold;
		fixed4 _EdgeColor;
		float _EdgeDampRate;
		float _OriginAlphaThreshold;
		float _EdgeRadius;
		int _ShowOutline;
		int _Painted;
		fixed4 _Color;
		fixed4 _PreColor;

		struct v2f
		{
			float4 vertex : SV_POSITION;
			//float2 uv/*[21*21]*/ : TEXCOORD0;
			float2 uv/*[21*21]*/ : TEXCOORD1;
		};

		float CalculateAlphaSumAround(v2f input)
		{
			half texAlpha;
			float alphaSum = 0;
			//for (int it = 0; it < 9; it++)
			//{
			//	texAlpha = tex2D(_MainTex, i.uv[it]).w;
			//	alphaSum += texAlpha;
			//}
			//half offset = 20;
			int index = 0;
			for (int i = -_EdgeRadius; i <= _EdgeRadius; i++) {
				for (int j = -_EdgeRadius; j <= _EdgeRadius; j++) {
					texAlpha = tex2D(_MainTex, input.uv + _MainTex_TexelSize.xy * half2(i, j)).w;
					alphaSum += texAlpha;
				}
			}
			alphaSum /= (_EdgeRadius * 2 + 1)*(_EdgeRadius * 2 + 1);

			return alphaSum;
		}

		v2f vert(appdata_img v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);

			o.uv = v.texcoord;

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 origin = tex2D(_MainTex, i.uv/*[center]*/);
			if (_Painted) {
				fixed4 res;// = _Color;// _PreColor + _Color;

				if (_ShowOutline) {
					res = _PreColor / 2 + _Color / 2; // _Color;
					res.a = 255;
				}
				else
					res = _PreColor;

				if (origin.a != 0)
					return res;
				return origin;
			}
			else {
				if (_ShowOutline /*&& _Painted*/) {
#if 0
					float alphaSum = CalculateAlphaSumAround(i);
					float isNeedShow = alphaSum > _EdgeAlphaThreshold;
					float damp = saturate((alphaSum - _EdgeAlphaThreshold) * _EdgeDampRate);
					fixed4 orign = tex2D(_MainTex, i.uv/*[center]*/);
					float isOrigin = orign.a > _OriginAlphaThreshold;
					//fixed3 finalColor = lerp(_EdgeColor.rgb, orign.rgb, isOrigon);
					fixed3 finalColor = lerp(_EdgeColor.rgb, _Color.rgb, isOrigin);

					return fixed4(finalColor.rgb, isNeedShow * damp);
#else
					if (origin.a != 0)
						return _Color;
					return origin;
#endif
				}
				else {
					return origin;
				}
			}

			//if(origin.a!=0)
			//	return _Color;// tex2D(_MainTex, i.uv/*[center]*/);

			//return origin;

		}

			ENDCG
		}
	}
}
