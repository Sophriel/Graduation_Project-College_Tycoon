Shader "Building Mask"
{
	SubShader{

		Tags{ "Queue" = "Background" }

		Cull off
		ColorMask 0
		ZWrite On
		ZTest LEqual

		Pass
		{
 	    }

	}
}