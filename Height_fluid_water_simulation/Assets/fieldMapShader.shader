Shader "CustomRenderTexture/Simple"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 0, 1)
        _Tex("InputTex", 2D) = "white" {}
     }

     SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
             #pragma target 3.0

            float4      _Color;
            sampler2D   _Tex;
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                
                float damping = 0.9f;
                
                float4 height_east = tex2D(_Tex, float2(min((IN.localTexcoord.x + 1.0f/_CustomRenderTextureWidth), 1.0f), IN.localTexcoord.y));
                float4 height_west = tex2D(_Tex, float2(max((IN.localTexcoord.x - 1.0f/_CustomRenderTextureWidth), 0.0f), IN.localTexcoord.y));
                float4 height_south = tex2D(_Tex, float2(IN.localTexcoord.x, min((IN.localTexcoord.y + 1.0f/_CustomRenderTextureWidth), 1.0f)));
                float4 height_north = tex2D(_Tex, float2(IN.localTexcoord.x, max((IN.localTexcoord.y - 1.0f/_CustomRenderTextureWidth), 0.0f)));
                float4 height_curr = tex2D(_Tex, IN.localTexcoord.xy);
                
                
                height_curr.g += ((height_west.r+ height_east.r + height_south.r + height_north.r)/4 - height_curr.r);
                height_curr.g *= damping;
                height_curr.r += height_curr.g;
                
                //r visina
                //g brzina
                
                //float4 col = tex2D(_Tex, IN.localTexcoord.xy);
                
                //col.r = 1.0f;
                //col.g = -col.g;
                
                
                //height_curr = float4(0.0f, 0.0f, 0.0f, 1.0f);
                
                if (IN.localTexcoord.y >= 0.5f && IN.localTexcoord.y <= 0.6f && IN.localTexcoord.x >= 0.5f && IN.localTexcoord.x <= 0.6f){
                    height_curr.r = 1.0f;
                    height_curr.g = 0.0f;
                }
                //else{
                //    height_curr = float4(0.0f, 0.0f, 0.0f, 1.0f);
                //}
                
                return height_curr;
                return float4(0.0f, 0.0f, 0.0f, 1.0f);
               
            }
            ENDCG
        }
    }
}