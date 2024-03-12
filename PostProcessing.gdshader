shader_type canvas_item;

uniform bool invert;
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment()
{
	vec3 col = textureLod(screen_texture, SCREEN_UV, 0.0).rgb;
	
	if(invert)
		col = 1.0 - col.rgb;
	
    COLOR = vec4(col, 1);
}