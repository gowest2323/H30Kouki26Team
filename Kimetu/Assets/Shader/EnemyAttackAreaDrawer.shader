Shader "Custom/EnemyAttackAreaDrawable" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Alpha("Alpha",Range(0.0,1.0)) = 0.8
        _AlphaBorder("AlphaBorder",Range(0.0,1.0)) = 0.2
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
        float _Alpha;
        float _AlphaBorder;

		void surf (Input IN, inout SurfaceOutput o) {            
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            //白に近いほどアルファ値が大きくなる
            _Color.a = _Alpha * (1.0f - (c.r * c.g * c.b));
            o.Albedo = _Color.rgb;
            //二次補間でアルファ値設定
            float t = _Color.a * _Color.a;
            clip(t - _AlphaBorder);
            o.Alpha = t;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
