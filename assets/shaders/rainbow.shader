shader_type canvas_item;

uniform float FALLING_SPEED = 1.0;
uniform float STRIPES_FACTOR = 1.0;

//get sphere
float sphere(vec2 coord, vec2 pos, float r) {
    vec2 d = pos - coord; 
    return smoothstep(60.0, 0.0, dot(d, d) - r * r);
}

//main
void fragment()
{
    //normalize pixel coordinates
    //vec2 uv = FRAGCOORD / (1.0 / SCREEN_PIXEL_SIZE).xy;
    //pixellize uv
    vec2 clamped_uv = round(vec2((FRAGCOORD.x / STRIPES_FACTOR), (FRAGCOORD.y / STRIPES_FACTOR)) * STRIPES_FACTOR) / (1.0 / SCREEN_PIXEL_SIZE).xy;
    //get pseudo-random value for stripe height
    float value		= fract(sin(clamped_uv.x) * 400.0);
    //create stripes
    vec3 col        = vec3(1.0 - mod(UV.y * 0.5 + (TIME * (FALLING_SPEED + value / 5.0)) + value, 0.5));
    //add color
         col       *= clamp(cos(TIME * 2.0 + UV.xyx + vec3(0, 2, 4)), 0.0, 1.0);
    //add glowing ends
    	 col 	   += vec3(sphere(vec2(FRAGCOORD.x, FRAGCOORD.y), 
                                  vec2(clamped_uv.x, (1.0 - 2.0 * mod((TIME * (FALLING_SPEED + value / 5.0)) + value, 0.5))) * (1.0 / SCREEN_PIXEL_SIZE).xy, 
                                  0.9)) / 2.0; 
    //add screen fade
         col *= vec3(exp(-pow(abs(UV.y - 0.5), 6.0) / pow(2.0 * 0.05, 2.0)));
		
    // Output to screen
    COLOR = vec4(col,1.0);
}
