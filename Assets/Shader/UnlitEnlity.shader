// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader"Unlit/UnlitEnlity"
{
    Properties
    {
        _Color ("Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #pragma instancing_options assumeuniformscaling

            #include "UnityCG.cginc"
            #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
            #include "UnityIndirect.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                //float4 color : COLOR0;
            };

            struct Entity
            {
                float2 Pos;
            };

            //float4 _Color;
            float4x4 _ObjectToWorld;
            StructuredBuffer<Entity> _PosBuffer;

            v2f vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                v2f o;
                InitIndirectDrawArgs(0);
                uint cmdID = GetCommandID(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);
                //float4 wpos = mul(_ObjectToWorld, v.vertex + float4(instanceID, cmdID, 0, 0));
                float4 wpos = mul(_ObjectToWorld, v.vertex + float4(_PosBuffer[instanceID].Pos, 0, 0));
                o.vertex = mul(UNITY_MATRIX_VP, wpos);
                //o.color = float4(_PosBuffer[instanceID].Pos, instanceID / float(GetIndirectInstanceCount()), 1.0);
                //o.color = float4(cmdID & 1 ? 0.0f : 1.0f, cmdID & 1 ? 1.0f : 0.0f, instanceID / float(GetIndirectInstanceCount()), 0.0f);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = i.color;
                return float4(0.3329654, 1, 0, 1);
                //return _Color;
            }
            ENDCG
        }
    }
Fallback"Diffuse"
}
