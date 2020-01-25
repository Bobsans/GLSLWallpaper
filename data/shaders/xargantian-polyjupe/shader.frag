#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;

#define MX (10.0 / resolution.y)
#define PI 3.141592
#define TAU PI * 2.0

vec3 getrd(vec3 ro, vec3 lookAt, vec2 uv) {
    vec3 dir = normalize(lookAt - ro);
    vec3 right = normalize(cross(vec3(0.0, 1.0, 0.0), dir));
    vec3 up = normalize(cross(dir, right));
    return dir + right * uv.x + up * uv.y;
}

float sdbox(vec3 p, vec3 s) {
    p = abs(p) - s;
    return max(p.x, max(p.y, p.z));
}

mat2 rot(float x) {
    return mat2(cos(x), -sin(x), sin(x), cos(x));
}

vec2 dmin(vec2 a, vec2 b) {
    return a.x < b.x ? a : b;
}

vec2 id;
vec2 map(vec3 p) {
    vec2 d = vec2(10e7);
    p.xy *= rot(0.0 + p.z * 0.1 + 0.1 * time);

    for (float i = 0.0; i < 4.0; i++) {
        p = abs(p);
        p.xy *= rot(0.4 * PI);
        p.x -= 0.2;
        p.x *= 1.0 + 0.4 * atan(p.x, p.y) / PI;
    }

    p.xy -= 2.0;
    p.y = abs(p.y);
    p.y -= 1.0 + sin(time * 0.1) * 0.2;

    id = floor(p.xz / 0.5);

    p.xy -= 0.8;
    p.xz = mod(p.xz, 0.5) - 0.25;

    for (float i = 0.0; i < 5.0; i++) {
        p = abs(p);
        p.y -= 0.28 - sin(time * 0.2) * 0.08 - 0.1;
        p.x += 0.04;
        p.xy *= rot(0.6 * PI + id.y * 6.0 + 0.9);
        if (i == 3.0) {
            p.xz *= rot(time * 2.0 + id.y);
        }
    }

    d = dmin(d, vec2(sdbox(p, vec3(0.125 + sin(time * 0.26) * 0.1)), 1.0));
    d.x *= 0.25;

    return d;
}

vec3 glow = vec3(0);

void main(void) {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution) / resolution.y;
    vec3 col = vec3(0.0);
    vec3 ro = vec3(0.0);
    ro.z += time * 3.0 + MX;

    float rate = ro.z * 0.1 + 0.1 * time;
    ro.xy += vec2(sin(rate), cos(rate)) * 2.0;

    vec3 lookAt = ro + vec3(0.0, 0.0, 4.0);
    float rotRate = time * 0.3;
    lookAt.xz += vec2(sin(rotRate), cos(rotRate));

    vec3 rd = getrd(ro, lookAt, uv);
    vec3 p = ro;
    float t = 0.0;

    for (int i = 0; i < 250; i++) {
        vec2 d = map(p);
        glow += exp(-d.x * 60.0) * (1.0 + 0.35 * cos(TAU * ((id.y * 0.01 + time * 0.2) * vec3(0.4, 0.5, 0.9) + (0.9 + p.z * 0.02))));

        if (d.x < 0.0005) {
            break;
        }
        if (t > 100.0) {
            break;
        }
        t += d.x;
        p = ro + rd * t;
    }

    fragColor = vec4(smoothstep(0.0, 1.0, mix(glow * 0.01, vec3(0.0), pow(clamp(t * 0.02 - 0.1, 0.0, 1.0), 2.0))), 1.0);
}
