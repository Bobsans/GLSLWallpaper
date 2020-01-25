#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

#define PI 3.141592

void main(void) {
    vec2 p = (gl_FragCoord.xy * 2.0 - resolution) / min(resolution.x, resolution.y);
    vec3 color = vec3(0.0, 0.3, 0.5);
    float d = 0.8 - smoothstep(0.0, 1.8, length(p));
    float f = 0.0;

    for (float i = 0.0; i < 16.0; i++) {
        float s = sin((mouse.x / resolution.x) * 0.715 * time + i * PI / 8.0) * 10.01;
        float c = cos((mouse.x / resolution.x) * 0.715 * time + i * PI / 8.0) * 8.5;
        f += mod(sin(time) * p.y + cos(time) * p.x, 1.0 - p.y) / (floor(-d - fract(12.0 * d) + 2.5) + p.x * c + fract(0.35 * time - 2.0 * d) + p.y * s);
    }

    fragColor = vec4(max(cos(acos((mouse.y / resolution.y) * 1.7 * p.y) * f + color), 1.0 - vec3(fract(2.0 * time), 0, 0.0) - (mod(f, fract(time + 12.0 * abs(p.x) - 2.0 * p.y)) + acos(1.05 * abs(p.y)))), 1.0);
}
