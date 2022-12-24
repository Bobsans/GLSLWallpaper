#version 330 core
precision highp float;

out vec4 fragColor;

uniform float time;
uniform vec2 resolution;

#define PI 3.141592654
#define TAU (2.0 * PI)
#define ROT(a) mat2(cos(a), sin(a), -sin(a), cos(a))
#define PSIN(x) (0.5 + 0.5 * sin(x))
#define LESS(a, b, c) mix(a, b, step(0.0, c))
#define SABS(x, k) LESS((0.5 / (k)) * (x) * (x) + (k) * 0.5, abs(x), abs(x) - (k))

const float fixed_radius2 = 1.9;
const float min_radius2 = 0.5;
const float folding_limit = 1.0;
const float scale = -2.8;

vec3 postProcess(vec3 col, vec2 q) {
    col = clamp(col, 0.0, 2.0);
    col = pow(col, vec3(1.0 / 2.2));
    col = col * 0.6 + 0.1 * col * col * (3.0 - 2.0 * col);
    col = mix(col, vec3(dot(col, vec3(0.33))), -0.4);
    col *= 0.5 + 0.5 * pow(19.0 * q.x * q.y * (1.0 - q.x) * (1.0 - q.y), 0.7);
    return col;
}

float pmin(float a, float b, float k) {
    float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
    return mix(b, a, h) - k * h * (1.0 - h);
}

float pmax(float a, float b, float k) {
    return -pmin(-a, -b, k);
}

float pabs(float a, float k) {
    return -pmin(-a, a, k);
}

float mod1(inout float p, float size) {
    float halfsize = size * 0.5;
    float c = floor((p + halfsize) / size);
    p = mod(p + halfsize, size) - halfsize;
    return c;
}

float modMirror1(inout float p, float size) {
    float halfsize = size * 0.5;
    float c = floor((p + halfsize) / size);
    p = mod(p + halfsize, size) - halfsize;
    p *= mod(c, 2.0) * 2.0 - 1.0;
    return c;
}

float vesica(vec2 p, float r, float d) {
    p = abs(p);
    float b = sqrt(r * r - d * d);
    return ((p.y - b) * d > p.x * b) ? length(p - vec2(0.0, b)) : length(p - vec2(-d, 0.0)) - r;
}

float unevenCapsule(vec2 p, float r1, float r2, float h) {
    p.x = abs(p.x);
    float b = (r1 - r2) / h;
    float a = sqrt(1.0 - b * b);
    float k = dot(p, vec2(-b, a));
    if (k < 0.0) {
        return length(p) - r1;
    }
    if (k > a * h) {
        return length(p - vec2(0.0, h)) - r2;
    }
    return dot(p, vec2(a, b)) - r1;
}

float parabola(vec2 pos, float wi, float he) {
    pos.x = abs(pos.x);
    float ik = wi * wi / he;
    float p = ik * (he - pos.y - 0.5 * ik) / 3.0;
    float q = pos.x * ik * ik * 0.25;
    float h = q * q - p * p * p;
    float r = sqrt(abs(h));
    float x = (h > 0.0) ? pow(q + r, 1.0 / 3.0) - pow(abs(q - r), 1.0 / 3.0) * sign(r - q) : 2.0 * cos(atan(r / q) / 3.0) * sqrt(p);
    x = min(x, wi);
    return length(pos - vec2(x, he - x * x / ik)) * sign(ik * (pos.y - he) + pos.x * pos.x);
}

float torus(vec3 p, vec2 t) {
    vec2 q = vec2(length(p.xz) - t.x, p.y);
    return length(q) - t.y;
}

float circle(vec2 p, float r) {
    return length(p) - r;
}

float segmenty(vec2 p, float off) {
    p.y = abs(p.y);
    p.y -= off;
    float d0 = abs(p.x);
    float d1 = length(p);
    return p.y > 0.0 ? d1 : d0;
}

vec2 toPolar(vec2 p) {
    return vec2(length(p), atan(p.y, p.x));
}

vec2 toRect(vec2 p) {
    return vec2(p.x * cos(p.y), p.x * sin(p.y));
}

void sphere_fold(inout vec3 z, inout float dz) {
    float r2 = dot(z, z);
    if (r2 < min_radius2) {
        float temp = (fixed_radius2 / min_radius2);
        z *= temp;
        dz *= temp;
    } else if (r2 < fixed_radius2) {
        float temp = (fixed_radius2 / r2);
        z *= temp;
        dz *= temp;
    }
}

vec3 polySoftMin3(vec3 a, vec3 b, vec3 k) {
    vec3 h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
    return mix(b, a, h) - k * h * (1.0 - h);
}

void box_fold(inout vec3 z, inout float dz) {
    const float k = 0.05;
    vec3 zz = sign(z) * polySoftMin3(abs(z), vec3(folding_limit), vec3(k));
    // z = clamp(z, -folding_limit, folding_limit);
    z = zz * 2.0 - z;
}

float sphere(vec3 p, float t) {
    return length(p) - t;
}

float mb(vec3 z) {
    vec3 offset = z;
    float dr = 1.0;
    float fd = 0.0;
    for (int n = 0; n < 5; ++n) {
        box_fold(z, dr);
        sphere_fold(z, dr);
        z = scale * z + offset;
        dr = dr * abs(scale) + 1.0;
        float r1 = sphere(z, 5.0);
        float r2 = torus(z, vec2(8.0, 1));
        r2 = abs(r2) - 0.25;
        float r = n < 4 ? r2 : r1;
        float dd = r / abs(dr);
        if (n < 2 || dd < fd) {
            fd = dd;
        }
    }
    return fd;
}

float smoothKaleidoscope(inout vec2 p, float sm, float rep) {
    vec2 hp = p;
    vec2 hpp = toPolar(hp);
    float rn = modMirror1(hpp.y, TAU / rep);
    float sa = PI / rep - SABS(PI / rep - abs(hpp.y), sm);
    hpp.y = sign(hpp.y) * (sa);
    hp = toRect(hpp);
    p = hp;
    return rn;
}

float eye(vec2 p) {
    float a  = mix(0.0, 0.85, smoothstep(0.995, 1.0, cos(TAU * time / 5.0)));
    const float b = 4.0;
    float rr = mix(1.6, b, a);
    float dd = mix(1.12, b, a);

    vec2 p0 = p;
    p0 = p0.yx;
    float d0 =  vesica(p0, rr, dd);
    float d5 = d0;

    vec2 p1 = p;
    p1.y -= 0.28;
    float d1 = circle(p1, 0.622);
    d1 = max(d1, d0);

    vec2 p2 = p;
    p2 -= vec2(-0.155, 0.35);
    float d2 = circle(p2, 0.065);

    vec2 p3 = p;
    p3.y -= 0.28;
    p3 = toPolar(p3);
    float n3 = mod1(p3.x, 0.05);
    float d3 = abs(p3.x) - 0.0125 * (1.0 - length(p1));

    vec2 p4 = p;
    p4.y -= 0.28;
    float d4 = circle(p4, 0.285);

    d3 = max(d3, -d4);

    d1 = pmax(d1, -d2, 0.0125);
    d1 = max(d1, -d3);

    float t0 = abs(0.9 * p.x);
    t0 *= t0;
    t0 *= t0;
    t0 *= t0;
    t0 = clamp(t0, 0.0, 1.0);
    d0 = abs(d0) - mix(0.0125, -0.0025, t0);


    float d = d0;
    d = pmin(d, d1, 0.0125);
    return d;
}

float starn(vec2 p, float r, float n, float m) {
    // next 4 lines can be precomputed for a given shape
    float an = 3.141593/float(n);
    float en = 3.141593/m;// m is between 2 and n
    vec2  acs = vec2(cos(an), sin(an));
    vec2  ecs = vec2(cos(en), sin(en));// ecs=vec2(0,1) for regular polygon

    float bn = mod(atan(p.x, p.y), 2.0*an) - an;
    p = length(p)*vec2(cos(bn), abs(sin(bn)));
    p -= r*acs;
    p += ecs*clamp(-dot(p, ecs), 0.0, r*acs.y/ecs.y);
    return length(p)*sign(p.x);
}

vec2 hand(vec2 p) {
    p.x = abs(p.x);
    vec2 p0 = p;
    p0 -= vec2(0.0, 0.180+0.00);
    float d0 = segmenty(p0, 0.61)-0.1;
    vec2 p1 = p;
    p1 -= vec2(0.2, 0.125);
    float d1 = segmenty(p1, 0.55)-0.09;
    vec2 p2 = p;
    p2 -= vec2(0.0, -0.38+0.3);
    p2.y = -p2.y;
    float d2 = unevenCapsule(p2, 0.3, 0.38, 0.3);
    vec2 p3 = p;
    p3 -= vec2(0.47, -0.31);
    float d3 = parabola(p3, 0.37, 0.5);

    vec2 p4 = p;
    p4 -= vec2(0.99, -0.4);
    float d4 = circle(p4, 0.61);
    d3 = max(d3, -d4);

    vec2 p5 = p;
    p5 -= vec2(0.0, -0.45);
    //  float d5 = vesica(p5.yx, 0.175, 0.1)-0.2;
    float d5 = starn(p5.yx, 0.33, 10.0, 3.5);
    float d6 = abs(d5-0.005)-0.005;


    d0 = min(d0, d1);
    d3 = p.y > -0.40 ? d3 : d2;


    float d = d3;
    d = min(d, d2);
    d = pmax(d, -(d0-0.01), 0.025);
    d = min(d, d0);
    float ds = max(min(d0, d3), -d5);
    d = max(d, -d6);

    float od = d;
    od = abs(od-0.02)-0.0075;

    d = min(d, od);
    d = pmin(d, d5, 0.01);
    return vec2(d, ds);
}

float weird(vec2 p) {
    const float s = 0.55;
    p /= s;
    float rep = 20.0;
    float ss = 0.05 * 6.0 / rep;
    vec3 p3 = vec3(p.x, p.y, PSIN(time * 0.257));
    p3.yz *= ROT(time * 0.05);
    float n = smoothKaleidoscope(p3.xy, ss, rep);
    return mb(p3) * s;
}

float df(vec2 p) {
    const float zw = 1.25;
    float da = weird(p/zw)*zw;
    vec2 dh = hand(p);
    const float ze = 0.28;
    vec2 pe = p;
    pe -= vec2(0.0, -0.45);
    pe /= ze;
    float de = eye(pe);
    de *= ze;

    float d = dh.x;
    d = max(d, -de);
    d = max(d, -pmax(da, dh.y, 0.0125));
    return d;
}

void main() {
    float aa = 2.0/resolution.y;
    vec2 q = gl_FragCoord.xy/resolution.xy;
    vec2 p = -1.0 + 2.0 * q;
    p.x    *= resolution.x/resolution.y;
    vec3 col = vec3(0.1*q.y);
    float d = df(p);
    float fade = smoothstep(0.0, 4.0, time);
    col = mix(col, mix(vec3(1.0, 0.5, 0.5), vec3(.5, 0.55, 0.95), q.y), smoothstep(aa, -aa, d)*fade);
    col = postProcess(col, q);
    fragColor = vec4(col, 1.0);
}
