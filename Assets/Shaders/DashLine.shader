Shader "Unlit/RoundedDashLine"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DashLength ("Dash Length", Range(0.1, 1)) = 0.5
        _GapLength ("Gap Length", Range(0.1, 1)) = 0.5
        _LineWidth ("Line Width", Range(0.01, 1)) = 0.1
        _Softness ("Softness", Range(0, 0.1)) = 0.02
        _RoundRadius ("Round Radius", Range(0, 1)) = 0.05
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DashLength;
            float _GapLength;
            float _LineWidth;
            float _Softness;
            float _RoundRadius;
            fixed4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Hàm tính khoảng cách đến hình chữ nhật bo tròn
            float roundedRectSDF(float2 p, float2 size, float radius) {
                float2 q = abs(p) - size + radius;
                return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - radius;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                
                // Tính chu kỳ dash
                float cycleLength = _DashLength + _GapLength;
                float cyclePos = fmod(uv.x, cycleLength);
                
                // Kiểm tra xem pixel hiện tại có nằm trong dash không
                float inDash = step(cyclePos, _DashLength);
                
                if (inDash < 0.5) {
                    discard; // Loại bỏ pixel nằm trong khoảng trống
                }
                
                // Tính vị trí trong dash hiện tại
                float dashPos = cyclePos / _DashLength;
                
                // Tạo tọa độ local cho dash (từ -1 đến 1)
                float2 localUV = float2(dashPos * 2.0 - 1.0, (uv.y - 0.5) * 2.0);
                
                // Kích thước của dash (chiều dài và chiều rộng)
                float2 dashSize = float2(1.0, _LineWidth);
                
                // Tính SDF cho hình chữ nhật bo tròn
                float dist = roundedRectSDF(localUV, dashSize, _RoundRadius);
                
                // Tạo alpha từ SDF với softness
                float alpha = 1.0 - smoothstep(-_Softness, _Softness, dist);
                
                // Áp dụng texture nếu có
                fixed4 texColor = tex2D(_MainTex, uv);
                
                return _Color * texColor * alpha;
            }
            ENDCG
        }
    }
}