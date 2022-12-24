#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;

const float count = 2.0;
const float speed = 0.3;

float hash(vec2 p, in float s) {
    return fract(sin(dot(vec3(p.xy, 27.0 * abs(sin(s))), vec3(27.1, 61.7, 12.4))) * 2.1);
}

float noise(in vec2 p, in float s) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f *= f * (3.0 - 2.0 * f);

    return mix(mix(hash(i + vec2(0.0), s), hash(i + vec2(1.0, 0.0), s), f.x), mix(hash(i + vec2(0.0, 1.0), s), hash(i + vec2(1.0), s), f.x), f.y) * s;
}

float fbm(vec2 p) {
    return -6.0 + noise(p * 1.0, 0.40) + noise(p * 2.0, 0.27) + noise(p * 4.0, 0.10) + noise(p * 8.0, 0.05);
}

void main() {
    float worktime = time * speed;
    vec2 uv = (gl_FragCoord.xy / resolution.xy) * 8.0;
    uv.x *= (resolution.x / resolution.y) * 0.7;

    float veryNoise = noise(uv, 0.5) * 3.0;
    float t1 = uv.y + fbm(uv + worktime) + veryNoise;
    float t2 = uv.y + fbm(uv + worktime / 2.0) + veryNoise;
    float line = smoothstep(0.3, 0.5, abs(0.3 / (t1 * 50.0)) * 3.0);

    if (t2 < t1) {
        for (float i = 2.0; i <= count; ++i) {
            line += smoothstep(0.1, 0.5, abs(0.3 / (((uv.y + fbm(uv + worktime / i)) + veryNoise) * (i * 50.0))) * i * 0.8);
        }
    }

    fragColor = vec4(clamp(line, 0.0, 0.18), clamp(line, 0.0, 0.71), clamp(line, 0.0, 0.95), 1.0);
}
