#version 330 core
precision highp float;

uniform float time;
uniform vec2 resolution;

float rand(vec2 uv) {
	return fract(sin(dot(uv, vec2(12.98348, 78.2425))) * 489304.0234);
}

void main( void ) {
	vec2 uv = gl_FragCoord.xy / resolution.xy;
	float t = rand(uv + vec2(time * rand(vec2(uv.x, time)), 0.0));

	gl_FragColor = vec4(t, t, t, 1.0);
}