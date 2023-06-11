#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

vec2 rand(vec2 st) {
    st = vec2(dot(st, vec2(127.1, 311.7)), dot(st, vec2(269.5, 183.3)));
    return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

void main() {
    vec2 sep = (gl_FragCoord.xy / min(resolution.x, resolution.y)) * vec2(12.0);
    vec2 fp = floor(sep);
    vec2 sp = fract(sep);
    float dist = 100.0;

    for (int y = -1; y <= 1; y++) {
        for (int x = -1; x <= 1; x++) {
            vec2 neighbor = vec2(x, y);
            vec2 p = sin(rand(fp + neighbor) * rand(fp + neighbor) * 100.0 + time) * 0.5 + 0.5;
            float disv = distance(neighbor + p, sp);
            dist = min(dist, disv);
        }
    }

    outColor = vec4(vec3(1.0) - dist, 1.0);
}
