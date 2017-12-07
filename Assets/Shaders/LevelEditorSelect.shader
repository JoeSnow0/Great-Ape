Shader "Unlit/LevelEditorSelect"
{
	Properties
	{
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 _OutlineColor;

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half4 col = texture2D(u_texture, v_texCoords);
				if (col.a > 0.5)
					half4 = col;
				else {
					float a = texture2D(u_texture, vec2(v_texCoords.x + offset, v_texCoords.y)).a +
						texture2D(u_texture, vec2(v_texCoords.x, v_texCoords.y - offset)).a +
						texture2D(u_texture, vec2(v_texCoords.x - offset, v_texCoords.y)).a +
						texture2D(u_texture, vec2(v_texCoords.x, v_texCoords.y + offset)).a;
					if (col.a < 1.0 && a > 0.0)
						half4 = half4(0.0, 0.0, 0.0, 0.8);
					else
						half4 = col;
			}
			ENDCG
		}
	}
}
