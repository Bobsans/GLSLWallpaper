#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

float hash(float x) {
    return fract(sin(x) * 48933.5453);
}

vec2 hash(vec2 p) {
    p = vec2(dot(p, vec2(127.1, 311.7)), dot(p, vec2(269.5, 183.3)));
    return fract(sin(p) * 43758.5453);
}

float mechstep(float x, float f, float r) {
    float fr = fract(x);
    float fl = floor(x);
    return fl + pow(fr, 0.5) + sin(fr * f) * exp(-fr * 3.5) * r;
}

vec3 voronoi(in vec2 x) {
    vec2 n = floor(x);
    vec2 f = fract(x);

    vec2 mg, mr;

    float md = 8.0;
    for (int j = -1; j <= 0; j ++) {
        for (int i = -1; i <= 0; i ++) {
            vec2 g = vec2(float(i), float(j));
            vec2 o = hash(n - g);
            vec2 r = g + o - f;
            float d = max(abs(r.x), abs(r.y));

            if (d < md) {
                md = d;
                mr = r;
                mg = g;
            }
        }
    }

    return vec3(n + mg, mr);
}

vec2 rotate(vec2 p, float a) {
    return vec2(p.x * cos(a) - p.y * sin(a), p.x * sin(a) + p.y * cos(a));
}

float stepfunc(float a) {
    return step(a, 0.0);
}

float fan(vec2 p, vec2 at, float ang) {
    p -= at;
    p *= 3.0;

    float v = 0.0, w, a;
    float le = length(p);

    v = le - 1.0;

    if (v > 0.0) {
        return 0.0;
    }

    a = sin(atan(p.y, p.x) * 3.0 + ang);

    w = le - 0.05;
    v = max(v, -(w + a * 0.8));

    w = le - 0.15;
    v = max(v, -w);

    return stepfunc(v);
}

float gear(vec2 p, vec2 at, float teeth, float size, float ang) {
    p -= at;
    float v = 0.0, w;
    float le = length(p);

    w = le - 0.3 * size;
    v = w;

    w = sin(atan(p.y, p.x) * teeth + ang);
    w = smoothstep(-0.7, 0.7, w) * 0.1;
    v = min(v, v - w);

    w = le - 0.05;
    v = max(v, -w);

    return stepfunc(v);
}

float car(vec2 p, vec2 at) {
    p -= at;
    float v = 0.0, w;
    w = length(p + vec2(-0.05, -0.32)) - 0.03;
    v = w;
    w = length(p + vec2(0.05, -0.32)) - 0.03;
    v = min(v, w);

    vec2 box = abs(p + vec2(0.0, -0.3 - 0.08));
    w = max(box.x - 0.1, box.y - 0.05);
    v = min(v, w);
    return stepfunc(v);
}

float layerA(vec2 p, float seed) {
    float v = 0.0, w, a;

    float si = floor(p.y);
    float sr = hash(si + seed * 149.91);
    vec2 sp = vec2(p.x, mod(p.y, 4.0));
    float strut = 0.0;
    strut += step(abs(sp.y), 0.3);
    strut += step(abs(sp.y - 0.2), 0.1);

    float st = time + sr;
    float ct = mod(st * 3.0, 5.0 + sr) - 2.5;

    v = step(2.0, abs(voronoi(p + vec2(0.35, seed * 194.9)).x));

    w = length(sp - vec2(-2.0, 0.0)) - 0.8;
    v = min(v, 1.0 - step(w, 0.0));

    a = st;
    w = fan(sp, vec2(2.5, 0.65), a * 40.0);
    v = min(v, 1.0 - w);

    return v;
}

float layerB(vec2 p, float seed) {
    float v = 0.0, w, a;

    float si = floor(p.y / 3.0) * 3.0;
    vec2 sp = vec2(p.x, mod(p.y, 3.0));
    float sr = hash(si + seed * 149.91);
    sp.y -= sr * 2.0;

    float strut = 0.0;
    strut += step(abs(sp.y), 0.3);
    strut += step(abs(sp.y - 0.2), 0.1);

    float st = time + sr;

    float cs = 2.0;
    if (hash(sr) > 0.5) {
        cs *= -1.0;
    }
    float ct = mod(st * cs, 5.0 + sr) - 2.5;

    v = step(2.0, abs(voronoi(p + vec2(0.35, seed * 194.9)).x) + strut);

    w = length(sp - vec2(-2.3, 0.6)) - 0.15;
    v = min(v, 1.0 - step(w, 0.0));
    w = length(sp - vec2(2.3, 0.6)) - 0.15;
    v = min(v, 1.0 - step(w, 0.0));
	
    if (v > 0.0) {
        return 1.0;
    }

    w = car(sp, vec2(ct, 0.0));
    v = w;

    if (hash(si + 81.0) > 0.5) {
        a = mechstep(st * 2.0, 20.0, 0.4) * 3.0;
    } else {
        a = st * (sr - 0.5) * 30.0;
    }

    w = gear(sp, vec2(-2.0 + 4.0 * sr, 0.5), 8.0, 1.0, a);
    v = max(v, w);

    w = gear(sp, vec2(-2.0 + 0.65 + 4.0 * sr, 0.35), 7.0, 0.8, -a);
    v = max(v, w);

    if (hash(si - 105.13) > 0.8) {
        w = gear(sp, vec2(-2.0 + 0.65 + 4.0 * sr, 0.35), 7.0, 0.8, -a);
        v = max(v, w);
    }

    if (hash(si + 77.29) > 0.8){
        w = gear(sp, vec2(-2.0 - 0.55 + 4.0 * sr, 0.30), 5.0, 0.5, -a + 0.7);
        v = max(v, w);
    }

    return v;
}

void main() {
    vec2 uv = (gl_FragCoord.xy / max(resolution.x, resolution.y)) * 2.0 - 1.0;
    vec2 p = uv * (resolution.x / resolution.y);
    vec2 cam = vec2(sin(time) * 0.4, time);

    p = rotate(p, sin(time) * 0.07);

    vec2 o = vec2(0.0, time);
    float v = 0.0, w;

    float z = 3.0 - sin(time * 0.7) * 0.5;
    for (int i = 0; i < 5; i ++) {
        float zz = 0.3 + z;
        float f = zz * 2.0 * 0.9;

        if (i == 3 || i == 1) {
            w = layerA(vec2(p.x, p.y) * f + cam, float(i));
        } else {
            w = layerB(vec2(p.x, p.y) * f + cam, float(i));
        }

        v = mix(v, exp(-abs(zz) * 0.16 + 0.1), w);
        z -= 0.8;
    }

    v = 1.0 - v;

    outColor = vec4(v, v, v, 1.0);
}
