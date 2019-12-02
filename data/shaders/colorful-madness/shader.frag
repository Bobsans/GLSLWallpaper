#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

#define time (time * 0.04 + 100.0)
#define PI 3.14159265358979323846

float box(vec2 _st, vec2 _size, float _smoothEdges, float tdist) {
    _size = tdist * vec2(1.75) - _size * 0.75;
    vec2 aa = vec2(_smoothEdges);
    vec2 uv = smoothstep(_size, _size + aa, _st);
    uv *= smoothstep(_size, _size + aa, vec2(1.0) - _st);
    return uv.x * uv.y;
}

vec2 tile(vec2 _st, float _zoom) {
    _st *= _zoom;
    return fract(_st);
}

vec2 rotate2D(vec2 _st, float _angle, vec2 shift) {
    _st -= 0.5 + shift.x;
    _st = mat2(cos(_angle), -sin(_angle), sin(_angle), cos(_angle)) * _st;
    _st += 0.5 + shift.y;
    return _st;
}

void main(void) {
    vec2 v = (gl_FragCoord.xy - resolution / 2.0) / min(resolution.y, resolution.x) * 5.0;
    float tm = time * 0.05;
    vec2 mspt = (vec2(sin(tm) + cos(tm * 0.2) + sin(tm * 0.5) + cos(tm * -0.4) + sin(tm * 1.3), cos(tm) + sin(tm * 0.1) + cos(tm * 0.8) + sin(tm * -1.1) + cos(tm * 1.5))) * 0.6;
    vec2 resV = gl_FragCoord.xy / resolution;
    float bdist = clamp(1.5 - 6.0 * distance(mouse.xy, resV), 0.0, 1.0);
    float R = 0.0;
    float RR = 0.0;
    float RRR = 0.0;
    float a = (0.6 - mspt.x) * 6.2;
    float C = cos(a);
    float S = sin(a);
    vec2 xa = vec2(C, -S);
    vec2 ya = vec2(S, C);
    vec2 shift = vec2(1.2, 1.62);
    float Z = 1.0 + mspt.y * 6.0;
    vec2 b = rotate2D(gl_FragCoord.xy, PI * Z, 0.05 * xa);

    for (int i = 0; i < 25; i++) {
        float br = dot(b, b);
        float r = dot(v, v);

        if (r > sin(tm) + 3.0) {
            r = (sin(tm) + 3.0) / r;
            v *= r;
        }

        if (br > 0.75) {
            br = 0.56 / br;
        }

        R *= 1.05;
        R += br;

        if (i < 24) {
            RR *= 1.05;
            RR += br;

            if (i < 23) {
                RRR *= 1.05;
                RRR += br;
            }
        }

        v = vec2(dot(v, xa), dot(v, ya)) * Z + shift;
        b = vec2(box(v, vec2(6.0), 0.0, bdist)) + shift * 0.42;
    }

    float c = mod(R, 2.0) > 1.0 ? 1.0 - fract(R) : fract(R);
    float cc = mod(RR, 2.0) > 1.0 ? 1.0 - fract(RR) : fract(RR);
    float ccc = mod(RRR, 2.0) > 1.0 ? 1.0 - fract(RRR) : fract(RRR);

    float blackout = 2.4 - 2.5 * bdist;

    if (blackout > 3.5) {
        blackout = 3.4 - (-1.2 + bdist);
    }

    if (ccc + cc + c < blackout) {
        float diff = ccc + cc + c - blackout;
        float m = -0.075;
        ccc = ccc - m * diff;
        cc = cc - m * diff;
        c = c - m * diff;
    }

    fragColor = vec4(ccc, cc, c, 1.0);
}
