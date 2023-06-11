#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

#define bx vec3(3.0, 3.0, 3.0)
#define size 0.45
#define inner 0.42

mat2 rot(float a) {
    float ca = cos(a);
    float sa = sin(a);
    return mat2(ca, -sa, sa, ca);
}

float hash12(vec2 p) {
    vec3 p3  = fract(vec3(p.xyx) * 0.1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return fract((p3.x + p3.y) * p3.z);
}

float box(vec3 p, vec3 b) {
    vec3 q = abs(p) - b;
    return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

float sub(float d1, float d2){
    return max(-d1, d2);
}

void q(inout vec3 p, in float rnd) {
    p.yz += 0.35 * vec2(cos(time + rnd * 333.0), sin(time * 2.0 + rnd * 333.0));
    p.x += sign(rnd - 0.5) * time * (rnd + 0.4) + rnd * 3.0;
}

float map(vec3 p) {
    float d = 999.0;
    vec3 pp = p;

    vec2 id = floor(p.zy / bx.xy);
    float rnd = hash12(id * 733.3);

    q(p, rnd);

    p = mod(p, bx) - bx * 0.5;

    p.xy *= rot(rnd * 23.0 + time);
    p.xz *= rot(rnd * 73.0 + time);

    d = min(box(p, vec3(size)), 4.7 - abs(pp.x));

    if (rnd > 0.6) {
        d = sub(box(p, vec3(inner, inner, 2.2)), d);
        d = sub(box(p, vec3(inner, 2.2, inner)), d);
        d = sub(box(p, vec3(2.2, inner, inner)), d);
    }

    return d;
}

void main() {
    vec2 uv = vec2(gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    vec3 rd = normalize(vec3(uv, 0.8));
    vec3 ro = vec3(-1.8, 0.5, 0.0);

    rd.xy *= rot(-0.3);
    rd.xz *= rot(-0.2);
    rd.yz *= rot(0.7);

    ro.z += time * 1.3;

    float d = 0.0, t = 0.0, ns = 0.0;

    for (int i = 0; i < 80; i++) {
        d = map(ro + rd * t);

        if (d < 0.002 || t > 40.0) {
            break;
        }
        t += d * 0.55;
        ns++;
    }

    vec3 p = ro + rd * t;
    vec3 col = pow(mix(vec3(0.05), vec3(0.001), exp(-t * t * t * 0.0001)) * 3.4, vec3(1.6));

    outColor = vec4(pow(max(col, 0.0), vec3(1.0 / 2.2)), 1);
}
