#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

#define PI 3.14159

float vDrop(vec2 uv, float t) {
    uv.x = uv.x * 128.0;
    float dx = fract(uv.x);
    uv.x = floor(uv.x);
    uv.y *= 0.05;
    float o=sin(uv.x * 215.4);
    float s=cos(uv.x * 33.1) * 0.3 + 0.7;
    float trail = mix(95.0, 35.0, s);
    float yv = fract(uv.y + t * s + o) * trail;
    yv = 1.0 / yv;
    yv = smoothstep(0.0, 1.0, yv * yv);
    yv = sin(yv * PI) * (s * 5.0);
    float d2 = sin(dx * PI);
    return yv * (d2 * d2);
}

void main() {
    vec2 p = (gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    float d = length(p) + 0.1;
    p = vec2(atan(p.x, p.y) / PI, 2.5 / d);

    if (mouse.z > 0.5) {
        p.y *= 0.5;
    } else if (mouse.w > 0.5) {
        p.y /= 0.5;
    }

    float t =  time * 0.4;
    vec3 col = vec3(1.55, 0.65, .225) * vDrop(p, t) + vec3(0.55, 0.75, 1.225) * vDrop(p, t + 0.33) + vec3(0.45, 1.15, 0.425) * vDrop(p, t + 0.66);

    fragColor = vec4(col * (d * d), 1.0);
}
