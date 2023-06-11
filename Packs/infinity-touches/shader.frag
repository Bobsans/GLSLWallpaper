#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

#define PI    3.14159265358
#define PI2   6.28318530718

const float scale = 1.0;
const vec3 d = vec3(0.957, 0.439, 0.043);

mat2 rot(float a) {
    return mat2(cos(a), sin(a), -sin(a), cos(a));
}

float hash21(vec2 a) {
    return fract(sin(dot(a, vec2(27.609, 57.583))) * 43758.5453);
}

vec3 hue(float t) {
    return 0.42 + 0.425 * cos(PI2 * t * (vec3(0.95, 0.97, 0.98) * d));
}

void main() {
    vec2 uv = (2.0 * gl_FragCoord.xy - resolution.xy) / max(resolution.x, resolution.y);
    vec3 C = vec3(0);
    vec2 vuv = uv, dv = uv;

    uv *= rot(time * 0.025);
    uv = vec2(log(length(uv)), atan(uv.y, uv.x)) * 3.5;
    uv.x -= time * 0.25;

    dv = vec2(log(length(dv)), atan(dv.y, dv.x)) * 3.5;
    dv.x += time * 0.075;

    float px = fwidth(uv.x);
    vec2 id = floor(uv * scale);
    float chk = mod(id.y + id.x, 2.0) * 2.0 - 1.0;

    float rnd = hash21(id);
    if (rnd > 0.5) {
        uv.x *= -1.0;
    }

    vec2 qv = fract(uv * scale) - 0.5;

    float circle = min(length(qv - vec2(-0.5, 0.5)) - 0.5, length(qv - vec2(0.5, -0.5)) - 0.5);
    float circle3 = abs(circle) - 0.05;

    float c2 = smoothstep(-px, px, abs(abs(circle) - 0.125) - 0.025);
    circle = (rnd > 0.5 ^^ chk > 0.5) ? smoothstep(px, -px, circle) : smoothstep(-px, px, circle);
    circle3 = smoothstep(0.125, -px, circle3);

    dv = fract(dv * scale) - 0.5;
    float dots = min(length(abs(dv) - vec2(0, 0.5)) - 0.25, length(abs(dv) - vec2(0.5, 0)) - 0.5);

    dots = abs(abs(abs(abs(dots) - 0.1) - 0.05) - 0.025) - 0.0125;
    dots = smoothstep(px, -px, dots);

    float hs = hash21(vuv) * 0.25;
    C = clamp(hue(52.0 + id.x * 0.15) + hs, C, vec3(1));

    C = mix(C, C * 0.75, dots);
    C = mix(C, C * 0.75, circle3);
    C = mix(C, vec3(0.001), clamp(min(circle, c2), 0.0, 1.0));

    C = pow(C, vec3(0.4545));
    outColor = vec4(C, 1.0);
}
