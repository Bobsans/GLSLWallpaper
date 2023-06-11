#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

float it = 2.0;

mat2 rot(float a) {
    float s = sin(a) ,c = cos(a);
    return mat2(c, s, -s, c);
}

float hash(vec2 p) {
    vec3 p3 = fract(vec3(p.xyx) * 0.1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return fract((p3.x + p3.y) * p3.z);
}

float de(vec3 p) {
    p.yz *= rot(-0.5);
    p.xz *= rot(time * 0.2);
    float d = 100.0;
    p *= 0.2;

    for (float i = 0.0; i < 12.0; i++) {
        p.xy = sin(p.xy * 2.0);
        p.xy *= rot(1.0);
        p.xz *= rot(1.5);
        float l = length(p.xy) + 0.01;

        if (i > 1.0) {
            d = min(d, l);
        }

        if (d == l) {
            it = i;
        }
    }
    return d * 0.3;
}

vec3 march(vec3 from, vec3 dir) {
    float d, td = hash(gl_FragCoord.xy + time) * 0.2;
    vec3 p, col = vec3(0.0);

    for (int i = 0; i < 200; i++) {
        p = from + dir * td;
        d = max(0.005, abs(de(p)));
        td += d;

        if (td > 10.0) {
            break;
        }

        vec3 c = vec3(1.0, -0.5, 0.0);
        c.rb *= rot(-it * 0.15 + time * 0.1);
        c = normalize(1.0 + c);
        c *= exp(-0.15 * td);
        c *= exp(-0.5 * length(p));
        c /= 1.0 + d * 1500.0;
        c *= 0.3 + abs(pow(abs(fract(length(p) * 0.15 - time * 0.2 + it * 0.02) - 0.5) * 2.0, 30.0)) * 4.0;
        col += c;
        col += exp(-5.0 * length(p)) * 0.15;
    }

    return col;
}

void main() {
    vec2 uv = (gl_FragCoord.xy - resolution.xy * 0.5) / resolution.y;
    vec3 from = vec3(0.0, 0.0, -3.0 - cos(time * 0.5));
    vec3 dir = normalize(vec3(uv, 1.2));
    outColor = vec4(march(from, dir), 1.0);
}