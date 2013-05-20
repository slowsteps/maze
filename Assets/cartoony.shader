Shader "Custom/cartoony" {
	Properties {
	 	_Color ("Main Color", Color) = (1,.5,.5,1)
	 	_Emission ("Emissive color", Color) = (0,0,0)

	}
	SubShader {
		
		Pass {
            Material {
                Diffuse (0,1,0,0)
                Ambient (1,0,0,0)
                Emission [_Emission]
            }
            Lighting On
        }
		

	} 
	FallBack "Diffuse"
}
