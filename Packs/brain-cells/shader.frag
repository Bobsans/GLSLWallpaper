#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;

#define TAU 3.14159 * 2.0

mat2 rz2(float a) {
    float c = cos(a), s = sin(a);
    return mat2(c, s, -s, c);
}

float cyl(vec2 p, float r) {
    return length(p) - r;
}

float cube(vec3 p, vec3 r) {
    return length(max(abs(p) - r, 0.0));
}

vec2 path(float z) {
    float x = sin(z) - 4.0 * cos(z * 0.3) - 0.5 * sin(z * 0.12345);
    float y = cos(z) - 4.0 * sin(z * 0.3) - 0.5 * cos(z * 2.12345);
    return vec2(x, y);
}

vec2 path2(float z) {
    float x = sin(z) + 4.0 * cos(z * 0.3) + 0.5 * sin(z * 0.12345);
    float y = cos(z) + 4.0 * sin(z * 0.3) + 0.3 * cos(z * 2.12345);
    return vec2(x, y);
}

vec2 modA(vec2 p, float count) {
    float an = TAU / count;
    float a = atan(p.y, p.x) + an * 0.5;
    a = mod(a, an) - an * 0.5;
    return vec2(cos(a), sin(a)) * length(p);
}

float smin(float a, float b, float r) {
    float h = clamp(0.5 + 0.5 * (b - a) / r, 0.0, 1.0);
    return mix(b, a, h) - r * h * (1.0 - h);
}

float GetDist(vec3 p) {
    p -= vec3(path(p.z) / 4.0, 0.0);
    vec3 q = p - vec3(path2(p.z) / 4.0, 0.0);
    p.xy *= rz2(p.z * sin(time * 0.002 + 250.0));
    q.xy *= rz2(q.z * sin(-time * 0.002 + 250.0));

    float cyl2wave = 0.3 + 1.5 * (sin(p.z + time * 0.5) * 0.1);
    float cylfade = 2.0 - smoothstep(0.0, 8.0, abs(p.z));
    float cyl2 = cyl(modA(p.xy, (abs(sin(time * 0.1) + 3.0))) - vec2(cyl2wave, 0.0) * cylfade, 0.02 * cyl2wave * cylfade);
    vec3 cubP = vec3(p.xy * rz2(time * 3.0), mod(p.z, 0.1) - 0.1);

    float cyl2a = smin(cyl2, cube(cubP, vec3(0.1 * cyl2wave * cylfade)), 0.5);

    cyl2wave = 0.3 + 1.5 * (sin(q.z + time * 0.5) * 0.1);
    cylfade = 2.0 - smoothstep(0.0, 8.0, abs(q.z));
    cyl2 = cyl(modA(q.xy, (abs(sin(time * 0.1) + 3.0))) - vec2(cyl2wave, 0.0) * cylfade, 0.06 * cyl2wave * cylfade);
    cubP = vec3(q.xy * rz2(time * 3.0), mod(q.z, 0.1) - 0.1);

    float cyl22 = smin(cyl2, cube(cubP, vec3(0.1 * cyl2wave * cylfade)), 0.5);

    return smin(cyl2a, cyl22, 0.5);
}

float marchCount;

float RayMarch(vec3 ro, vec3 rd) {
    float dO = 0.0;
    marchCount = 0.0;

    for (int i = 0; i < 68; i++) {
        vec3 p = ro + dO * rd;
        float dS = GetDist(p);
        dO += dS;
        if (dS < 0.001 || dO > 100.0) {
            break;
        }
        marchCount += 1.0 / dS * 0.02;
    }

    return dO;
}

vec3 GetNormal(vec3 p) {
    float d = GetDist(p);
    vec2 e = vec2(0.1, 0);
    vec3 n = d - vec3(GetDist(p - e.xyy), GetDist(p - e.yxy), GetDist(p - e.yyx));

    return normalize(n);
}

void main() {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    vec3 ro = vec3(0.0, 0.0, -time);
    vec3 rd = normalize(vec3(uv, 1.0));
    float the = time * 0.01;
    rd.yx *= mat2(cos(the), -sin(the), sin(the), cos(the));
    float d = RayMarch(ro, rd);
    vec3 p = ro + rd * d;
    vec3 fog = vec3(1.2 / (1.0 + d * d * 0.3));

    fragColor = vec4((fog + (marchCount * vec3((cos(time + -p.z) * 2.0), 0.15, 0.0) * 0.02)) * fog, 1.0);
}
