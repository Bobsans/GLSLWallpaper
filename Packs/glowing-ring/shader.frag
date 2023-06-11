#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;
uniform vec2 mouse;

const float lineNum = 16.0;
const float variation = 0.5;
const float speed = 0.01;
const vec3 baseColor = vec3(1.0, 0.5, 0.1);
const float glowing = 0.007;
const float glowSpeed = 1.5;
const float petalNum = 6.0;

void main() {
	vec2 pos = (gl_FragCoord.xy * 2.0 - resolution.xy) / min(resolution.x, resolution.y) * 0.5;
	float radius = length(pos) * 3.0 - 1.0;
	float t = atan(pos.y + variation, pos.x);
	float color = 0.0;
	
	for (float i = 1.0; i <= lineNum; i++) {
		color += glowing * (cos(time * glowSpeed) / 3.0 + 1.0) / abs(0.2 * sin(petalNum * (t + i * time * speed)) - radius);
	}
	
	outColor = vec4(baseColor * color, 1.0);
}
