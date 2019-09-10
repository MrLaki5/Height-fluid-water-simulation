Shader "CustomRenderTexture/Simple"
{
    Properties
    {
        _Wave_height("Wave height", Range(0.1, 1)) = 0.5
        _Tex("InputTex", 2D) = "white" {}
        _Click_x("Click X", Range(-1, 1)) = -1.0
        _Click_y("Click Y", Range(-1, 1)) = -1.0
        _Box_low_x("Box low X", Range(-1, 1)) = -1.0
        _Box_low_y("Box low Y", Range(-1, 1)) = -1.0
        _Box_high_x("Box high X", Range(-1, 1)) = -1.0
        _Box_high_y("Box high Y", Range(-1, 1)) = -1.0
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
             
            sampler2D   _Tex;
            float _Click_x;
            float _Click_y;
            float _Box_low_x;
            float _Box_low_y;
            float _Box_high_x;
            float _Box_high_y;
            float _Wave_height;
            
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
                float range = 0.005f;
                if (_Click_x != -1.0 && _Click_y != -1.0 && (abs(IN.localTexcoord.y - _Click_y) <= range) && (abs(IN.localTexcoord.x - _Click_x) <= range))
                {
                    height_curr.r = _Wave_height;
                    height_curr.g = 0.0f;
                }
                
                if (_Box_low_x != -1.0 && _Box_low_y != -1.0 && _Box_high_x != -1.0 && _Box_high_y != -1.0 && (IN.localTexcoord.x >= _Box_low_x && IN.localTexcoord.x <= _Box_high_x) && (IN.localTexcoord.y >= _Box_low_y && IN.localTexcoord.y <= _Box_high_y))
                {
                    height_curr.r = _Wave_height;
                    height_curr.g = 0.0f;  
                }
                
                //else{
                //    height_curr = float4(0.0f, 0.0f, 0.0f, 1.0f);
                //}
                
                return height_curr;
                //return float4(0.0f, 0.0f, 0.0f, 1.0f);
               
            }
            ENDCG
        }
    }
}