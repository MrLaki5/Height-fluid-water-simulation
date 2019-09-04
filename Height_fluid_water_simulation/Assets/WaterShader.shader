Shader "Custom/WaterShader"
{
    Properties
    {
        _CubeMap ("Cube map", CUBE) = "white" {}
        _WaterColor ("Water color", Color) = (90, 188, 216)
        _Transparency("Transparency", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "LightMode" = "ForwardBase" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        ZWrite off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal : NORMAL;
                float3 vertexPosInObjSpace : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            samplerCUBE _CubeMap;
            float _Transparency;
            float4 _WaterColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.vertexPosInObjSpace = v.vertex;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate view dir vector in object space
                float3 viewDir = normalize(ObjSpaceViewDir(float4(i.vertexPosInObjSpace, 1))).xyz;
                // We get reflection with reflecting vector that gets out of fragment to camera over normal of fragment.
                float3 uv = reflect( -viewDir, normalize(i.normal));
                // Move it from object space to world space
                uv = mul(UNITY_MATRIX_M, float4(uv, 0));
                // Sample cube map with vector we got
                fixed4 col = texCUBE(_CubeMap, uv);
                // If angle between view and normal is smaller, alfa is bigger
                col.a = (1-dot(viewDir, i.normal));
                col = col * _WaterColor;
                return col;
            }
            ENDCG
        }
    }
}

