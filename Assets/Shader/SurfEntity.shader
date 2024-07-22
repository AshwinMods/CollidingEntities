Shader "Unlit/SurfEntity"
{
	Properties 
	{
        _Color ("Color", color) = (1,1,1,1)
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:setup

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		//float4 _Color;
		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				StructuredBuffer<float3> _PosBuffer;
		#endif
		void setup()
		{
			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				float3 data = _PosBuffer[unity_InstanceID];
				unity_ObjectToWorld._14_24_34_44 = float4(data.xyz, 1);
			#endif
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			//o.Albedo = _Color.rgb;
			//o.Alpha = _Color.a;
			o.Albedo = float3(0.3329654, 1, 0);
			o.Alpha = 1;
		}
		ENDCG
	}
}
