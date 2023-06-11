#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

void main() {
    vec2 pos = 256.0 * gl_FragCoord.xy / resolution.x + time;
    vec3 col = vec3(0.0);

    for (int i = 0; i < 6; i++) {
        vec2 a = floor(pos);
        vec2 b = fract(pos);
        vec4 w = fract((sin(a.x * 7.0 + 31.0 * a.y + 0.01 * time) + vec4(0.035, 0.01, 0.0, 0.7)) * 13.545317);

        col += w.xyz * smoothstep(0.45, 0.55, w.w) * sqrt(16.0 * b.x * b.y * (1.0 - b.x) * (1.0 - b.y));
        pos /= 2.0;
        col /= 2.0;
    }

    outColor = vec4(pow(2.5 * col, vec3(1.0, 1.0, 0.7)), 1.0);
}
