#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

#define PI    3.14159265358
#define PI2   6.28318530718

mat2 rot(float a) {
    return mat2(cos(a), sin(a), -sin(a), cos(a));
}

float hash21(vec2 a) {
    return fract(sin(dot(a, vec2(22.34, 35.34))) * 483434.0);
}

vec2 hash22(vec2 uv) {
    return fract(sin(uv * 425.215 + uv.yx * 714.388) * vec2(522.877));
}

float opx(in float d, in float z, in float h) {
    vec2 w = vec2(d, abs(z) - h);
    return min(max(w.x, w.y), 0.0) + length(max(w, 0.0));
}

vec3 hp, hitpoint;
float glow = 0.0, t12 = 0.0;

const float size = 5.25;
const float hlf =size / 2.0;

void sep(inout vec3 p) {
    if (p.x + p.y < 0.0) {
        p.xy = -p.yx;
    }
    if (p.x + p.z < 0.0) {
        p.xz = -p.zx;
    }
    if (p.y + p.z < 0.0) {
        p.zy = -p.yz;
    }
}

vec2 map(vec3 p, float sq) {
    vec2 res = vec2(1e5, 0.0);
    p.y += 3.0;
    p.z -= t12;
    vec3 o = p;
    o.y += 0.5 * sin(time * 0.21 + o.z * 0.15) + 0.7 * cos(time * 0.12 + o.x * 0.3);

    vec2 id = floor((p.xz + hlf) / size);
    p.xz = mod(p.xz + hlf, size) - hlf;

    float hs = 2.50 * sin(id.x + id.y * 2.0 + time * 0.3);
    float ht = 2.25 * sin(id.x * 2.0 + id.y * 1.3 + time * 1.3);

    float ss = hash21(id);
    p.y += ht;
    vec3 q = p;

    p.zy *= rot(hs * PI2 + (time * 0.04));
    p.xy *= rot(hs * PI2 - (time * 0.05));

    if (ss > 0.25) {
        sep(p);
    }
    float fs = hash21(id + vec2(22.0));
    if (fs > 0.5) {
        p = abs(p.zyx);
    } else if (fs > 0.75) {
        p = abs(p.yzx);
    }

    float r = length(p.xy) - 1.45;
    r = abs(r) - 0.1;
    float d = opx(r, abs(p.z) - 0.25, 0.05) / 1.25;
    if (d < res.x) {
        res = vec2(d, 1.0);
    }

    float d2 = length(q) - (1.15 - ss);
    if (d2 < res.x) {
        res = vec2(d2, 2.0);
    }

    float ms = hash21(id + floor(time * 1.5));
    if (sq == 1.0 && ms > 0.875) {
        glow += 0.002 / (0.0025 + d2 * d2);
    }

    return res;
}

vec3 normal(vec3 p, float t) {
    vec2 e = vec2(t * 1e-3, 0.0);
    float d = map(p, 0.0).x;
    vec3 n = d - vec3(map(p - e.xyy, 0.0).x, map(p - e.yxy, 0.0).x, map(p - e.yyx, 0.0).x);
    return normalize(n);
}

vec3 shade(vec3 p, vec3 n, vec3 ro, float m) {
    vec3 l = normalize(vec3(-2, 15, -10) - p);
    float diff = clamp(dot(n, l), 0.1, 1.0);

    float shdw = 1.0;
    for (float t = 0.01; t < 18.0; t += 0.5) {
        float h = map(p + l * t, 0.).x;
        if (h < 1e-4) {
            shdw = 0.0;
            break;
        }
        shdw = min(shdw, 18.0 * h / t);
        if (shdw < 1e-4 || t + h > 18.0) {
            break;
        }
    }
    diff = mix(diff, diff * shdw, 0.85);

    float spec = 0.15 * pow(max(dot(normalize(p - ro), reflect(l, n)), 0.0), 24.0);

    vec3 h = vec3(0);
    if (m == 1.0) {
        h = vec3(0.05);
    }
    if (m == 2.0) {
        h = vec3(0.75);
    }

    return h * diff + spec;
}

void main() {
    vec2 F = gl_FragCoord.xy;
    t12 = (time * 11.0);
    vec2 uv = (2.0 * F.xy - resolution.xy) / max(resolution.x, resolution.y);

    vec3 C = vec3(0);
    vec3 ro = vec3(0, -1.0, 12);
    vec3 rd = normalize(vec3(uv, -1.0));

    float dof = 0.001;
    float dofdist = 1.0 / 10.0;

    vec2 off = vec2(-0.05, 0.05);
    ro.xy += off * dof * smoothstep(0.0, 1.0, length(uv)) * 0.01;

    mat2 rx = rot(-0.60 - 0.46 * sin(time * 0.4));
    mat2 ry = rot(-0.178 * sin(time * 0.2) + pow(1.95, cos(time * 0.2) * PI));
    ro.zy *= rx;
    ro.xz *= ry;
    rd.zy *= rx;
    rd.xz *= ry;

    float fa = 0.0, sglow = 0.0;
    for (int k = 0; k < 2; k++) {
        vec3 p = ro;
        float d = 0.0, m = 0.0;
        vec3 RC = vec3(0);

        for (int i = 0; i < 100; i++) {
            //modified jitter/dof
            //inspiration @Nusan https://www.shadertoy.com/view/3sXyRN
            if (mod(float(i), 2.0) < 1.0) {
                //off= texture(iChannel0, F.xy / 1024.0).rg * 2.0 - 1.0;
                ro.xz += off * dof * (d) * 0.01;
                rd.xy += off * dof * (d * d) * dofdist * 0.012;
            }

            p = ro + d * rd;

            vec2 ray = map(p, 1.0);
            m = ray.y;
            d += i < 26 ? ray.x * 0.5 : ray.x * 0.8;
            if (ray.x < d * 1e-3 || d > 50.0) {
                break;
            }
        }

        if (k == 0) {
            fa = d;
            sglow = glow;
        }

        if (d < 50.0) {
            vec3 n = normal(p, d);
            RC += shade(p, n, ro, m);
            ro = p + n * 0.001;
            rd = reflect(rd, n);
        }

        if (k > 0) {
            RC *= 0.25;
        }

        C = clamp(C + RC, vec3(0), vec3(1));
    }

    C = mix(C, vec3(0.1), 1.0 - exp(-0.00003 * fa * fa * fa));
    C = mix(C, vec3(sglow, sglow * 0.5, sglow * 0.3), clamp(sglow, 0.0, 1.0));
    C = pow(C, vec3(0.4545));
    outColor = vec4(C, 1.0);
}
