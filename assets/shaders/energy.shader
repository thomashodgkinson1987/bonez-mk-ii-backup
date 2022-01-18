shader_type canvas_item;

void fragment()
{
	vec3 c;
	float l,z=TIME;
	
	vec2 uv,p=FRAGCOORD.xy/(1.0 / SCREEN_PIXEL_SIZE);
	uv = p;
	p = uv - 0.5;
	p.x*=(1.0 / SCREEN_PIXEL_SIZE).x/(1.0 / SCREEN_PIXEL_SIZE).y;
	z+=0.1;
	l=length(p);
	uv+=p/l*(sin(z)+1.0)*abs(sin(l*9.0-z*-1.0));
	c[0]=0.01/length(abs(mod(uv,1.0)-0.5));
	
	p = uv - 0.5;
	p.x*=(1.0 / SCREEN_PIXEL_SIZE).x/(1.0 / SCREEN_PIXEL_SIZE).y;
	z+=0.2;
	l=length(p);
	uv+=p/l*(sin(z)+1.0)*abs(sin(l*9.0-z*-1.0));
	c[1]=0.01/length(abs(mod(uv,1.0)-0.5));
	
	p=uv-0.5;
	p.x*=(1.0 / SCREEN_PIXEL_SIZE).x/(1.0 / SCREEN_PIXEL_SIZE).y;
	z+=0.3;
	l=length(p);
	uv+=p/l*(sin(z)+1.0)*abs(sin(l*9.0-z*-1.0));
	c[2]=0.01/length(abs(mod(uv,1.0)-0.5));
	
	COLOR=vec4(c/l,TIME);
}