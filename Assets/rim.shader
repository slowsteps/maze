Shader "Rim" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
      _Emission ("Emissive color", Color) = (0,0,0)
    }
    SubShader {
    	Pass {
            Material {
                Diffuse (0,1,0,0)
                Ambient (0,0,0,0)
                Emission [_Emission]
            }
            Lighting On
        }
      Tags { "RenderType" = "Opaque" }
      
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 viewDir;
      };
      sampler2D _MainTex;
      float4 _RimColor;
      float _RimPower;
      void surf (Input IN, inout SurfaceOutput o) {
          //o.Albedo = (0,1,0.3);
          //o.Albedo = [_Emission];
          //o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          //o.Emission = _RimColor.rgb * pow (rim, _RimPower);
          o.Emission = (0.5,1.0,0.5);
          o.Albedo = _RimColor.rgb * pow (rim, _RimPower);
      }
      ENDCG
      
    } 
    Fallback "Diffuse"
  }