// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/LinearGradient"
{
	Properties {
        _TopColor ("Top color", Color) = (1,1,1,1)
		_BottomColor ("Bottom color", Color) = (1,1,1,1)
    }
	SubShader
	{

		Tags
		{
			"RenderType"="Opaque"
			"LightMode"="ForwardBase"
		}
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "AutoLight.cginc"			

			fixed4 _TopColor;
			fixed4 _BottomColor;



			struct v2f {
				float4 position : SV_POSITION;
				SHADOW_COORDS(1) //TEXCOORD1
				float3 normal : NORMAL;
				fixed4 color : COLOR;
				float3 diff : COLOR1;
				fixed3 ambient : COLOR2;
			};

			v2f vert (appdata_full v)
			{
				v2f o;

				//normals
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.normal= UnityObjectToWorldNormal(v.normal);

				//light
				o.position = UnityObjectToClipPos (v.vertex);
				o.color = lerp(_TopColor,_BottomColor, v.texcoord.y );
				float nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));				
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{

				float4 color = i.color;
				fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;
				color.rgb *= lighting*2;
				color.a = 1;
				return color;
			}
			ENDCG
		}
	}
}
