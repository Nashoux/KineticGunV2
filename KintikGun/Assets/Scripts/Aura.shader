// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Aura"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_Color1 ("Aura 1 Color", Color) = (1,1,1,1)
		_AuraPower1 ("Aura 1 Transparence", Range(0,5)) = 1
		_Size1("Aura 1 Size", float) = 1.5
		_Scale1 ("Aura 1 Scale", float) = 1
		_Speed1 ("Aura 1 Speed", float) = 1
		_Frequency1 ("Aura 1 Frequency", float) = 1

		_Color2 ("Aura 2 Color", Color) = (1,1,1,1)
		_AuraPower2 ("Aura 2 Transparence", Range(0,5)) = 1
		_Size2 ("Aura 2 Size", float) = 1.5
		_Scale2 ("Aura 2 Scale", float) = 1
		_Speed2 ("Aura 2 Speed", float) = 1
		_Frequency2 ("Aura 2 Frequency", float) = 1

		_ColorFill ( "fillColor", Color ) = (0,0,0,1)
		_fillPourcent( "pourcent fill", float ) = -1
		_BoundsUp( "boundsSize Up", float ) = 0
		_BoundsDown( "boundsSize Down", float ) = 0

		_Wiggle("Wiggle power", float) = 2

		[Enum(Wave,1, NoWave,2)] _IsWaveActive("_IsWaveActive", Float) = 1 
		
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

            struct appdata
            {
                float4 vertex : POSITION;
				float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				SHADOW_COORDS(1) //TEXCOORD1
				float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
				float3 diff : COLOR0;
				fixed3 ambient : COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			uniform float4 _ColorFill; 
			uniform float _fillPourcent;
			uniform float _BoundsUp;
			uniform float _BoundsDown;
			uniform float _BoundsMiddle;

			float _IsItFill;
			
            v2f vert (appdata v)
            {
                v2f o;

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex); //convert to world
				if(worldPos.y < _BoundsDown + (_BoundsUp - _BoundsDown) * _fillPourcent){ // if the power change this pixel color or not
					_IsItFill = 1;
				}else{
					_IsItFill = 0;
				}
				o.vertex = mul(UNITY_MATRIX_VP, worldPos);//convert to camera

				//color
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//normals
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.normal= UnityObjectToWorldNormal(v.normal);

				//lights
                float nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                TRANSFER_SHADOW(o);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
				//color
                fixed4 col = tex2D(_MainTex, i.uv);

				//lights
				fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;

				if( _IsItFill == 1){ // if the power change this pixel color
					col.rgb += _ColorFill.rgb;
					col.rgb /=2;
					col.rgb *= lighting;
				}else{
                	col.rgb *= lighting;
				}				
				
                return col;
            }
            ENDCG
        }
		

		Tags
		{
			"RenderType"="Opaque"
			"Queue" = "Transparent"
		}
		LOD 200

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform fixed4 _Color1;
			uniform float _AuraPower1;
			uniform float _Size1;
			uniform float _Wiggle;
			uniform float _IsWaveActive;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Scale1, _Speed1, _Frequency1;
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};
			
			v2f vert (appdata v)
			{
				half offsetvert = v.vertex.x + v.vertex.z; //diagonal waves
				float value = _Scale1 * sin(_Time.w * _Speed1 * _Frequency1 + offsetvert);
				if (_IsWaveActive == 1){
					v.vertex.y += value; //remove for no waves
					v.normal.y += value; //remove for no waves
				}

				v2f o;

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex* _Size1); //convert to world
				worldPos.x += sin(worldPos.y/_Wiggle+_Time.w/_Wiggle);
				o.vertex = mul(UNITY_MATRIX_VP, worldPos); //convert to camera
				
				//normal
				o.normal= v.normal.xyz;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = _Color1;
				col.w =  abs(i.normal.x * i.normal.z) * _AuraPower1;
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

			uniform fixed4 _Color2;
			uniform float _AuraPower2;
			uniform float _Size2;
			uniform float _Wiggle;
			uniform float _IsWaveActive;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Scale2, _Speed2, _Frequency2;
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};
			
			v2f vert (appdata v)
			{
				half offsetvert = v.vertex.x + v.vertex.z; //diagonal waves
				float value = _Scale2 * sin(_Time.w * _Speed2 * _Frequency2 + offsetvert);
				if (_IsWaveActive == 1){
					v.vertex.y += value; //remove for no waves
					v.normal.y += value; //remove for no waves
				}

				v2f o;

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex* _Size2); //convert to world
				worldPos.xz += sin(worldPos.y/_Wiggle+_Time.w/_Wiggle); 
				o.vertex = mul(UNITY_MATRIX_VP, worldPos); //convert to camera
				
				//normal
				o.normal= v.normal.xyz;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = _Color2;
				col.w =  abs(i.normal.x * i.normal.z) * _AuraPower2;
				return col;
			}



			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	
	FallBack "Diffuse"
	
}