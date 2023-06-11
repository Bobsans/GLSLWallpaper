#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

float sdBezier(in vec2 pos, in vec2 A, in vec2 B, in vec2 C) {
    vec2 a = B - A;
    vec2 b = A - 2.0 * B + C;
    vec2 c = a * 2.0;
    vec2 d = A - pos;
    float kk = 1.0 / dot(b, b);
    float kx = kk * dot(a, b);
    float ky = kk * (2.0 * dot(a, a) + dot(d, b)) / 3.0;
    float kz = kk * dot(d, a);
    float res = 0.0;
    float p = ky - kx * kx;
    float p3 = p * p * p;
    float q = kx * (2.0 * kx * kx - 3.0 * ky) + kz;
    float h = q * q + 4.0 * p3;

    if (h >= 0.0) {
        h = sqrt(h);
        vec2 x = (vec2(h, -h) - q) / 2.0;
        vec2 uv = sign(x) * pow(abs(x), vec2(1.0 / 3.0));
        float t = uv.x + uv.y - kx;
        t = clamp(t, 0.0, 1.0);
        vec2 qos = d + (c + b * t) * t;
        res = dot(qos, qos);
    } else {
        float z = sqrt(-p);
        float v = acos(q / (p * z * 2.0)) / 3.0;
        float m = cos(v);
        float n = sin(v) * 1.732050808;
        vec3 t = vec3(m + m, -n - m, n - m) * z - kx;
        t = clamp(t, 0.0, 1.0);
        vec2 qos = d + (c + b * t.x) * t.x;
        res = dot(qos, qos);
        qos = d + (c + b * t.y) * t.y;
        res = min(res, dot(qos, qos));
        qos = d + (c + b * t.z) * t.z;
        res = min(res, dot(qos, qos));
    }

    return sqrt(res);
}

float opUnion(float d1, float d2) {
    return min(d1, d2);
}

float sdCircle(vec2 p, vec2 c, float r) {
    return length(p - c) - r;
}

float sdBox(vec2 p, vec2 c, vec2 b) {
    p -= c;
    vec2 d = abs(p) - b;
    return length(max(d, vec2(0))) + min(max(d.x, d.y), 0.0);
}

float sdLine(vec2 p, vec2 a, vec2 b) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
    return length(pa - ba * h);
}

float smin(float a, float b, float k) {
    float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
    return mix(b, a, h) - k * h * (1.0 - h);
}

vec2 translate(vec2 p, vec2 t) {
    return p - t;
}

float horseDist(vec2 uv) {
    float t = time * 17.0;
    vec2 body = vec2(0.0, 100.0);
    vec2 behind = body + vec2(-100.0, 20.0);
    vec2 front = body + vec2(100.0, 0.0);

    front += 20.0 * sin(t);
    behind += 30.0 * cos(t) * vec2(0.2, 1.3);

    vec2 tail_start = behind + vec2(-70.0, 5.0);
    vec2 tail_end = tail_start + vec2(-70.0, 35.0 * cos(t * 1.2));
    vec2 tail_mid = (tail_start+tail_end) / 2.0 + vec2(0.0, 1.0) * 50.0 * sin(t * 1.2);

    vec2 backleg_top = behind;
    vec2 backleg_mid = backleg_top + vec2(50.0 * cos(-t), -150.0);
    vec2 backleg_bottom = backleg_mid + vec2(50.0 * sin(t), -70.0 * sin(t));

    float ft = t + 1.9;
    vec2 frontleg_top = front;
    vec2 frontleg_mid = frontleg_top + vec2(50.0 * cos(-ft), -150.0);
    vec2 frontleg_bottom = frontleg_mid + vec2(50.0 * sin(ft), -70.0 * sin(t));

    vec2 head = front+vec2(100.0, 70.0);
    vec2 nose = head+vec2(100.0, -50.0);

    head.y += 15.0 * sin(t * 1.3 - 1.5);
    nose += 30.0 * sin(t * 1.1);

    vec2 eye_p = head;
    vec2 ears = head + vec2(-20.0, 30.0);
    vec2 ears_end = head + vec2(-70.0, 50.0) + sin(t * 1.1) * 10.0 * vec2(0.0, 1.0);

    eye_p += sin(t) * vec2(-5.0, -5.0);

    float d1 = sdCircle(uv, behind, 70.0);
    float d2 = sdCircle(uv, front, 50.0);
    float d3 = sdBox(uv, body, vec2(50.0, 50.0));

    float tail = sdBezier(uv, tail_start, tail_mid, tail_end) - 5.0;

    float backleg = sdBezier(uv, backleg_top, backleg_mid, backleg_bottom) - 15.0;
    float frontleg = sdBezier(uv, frontleg_top, frontleg_mid, frontleg_bottom) - 15.0;

    float neck = sdLine(uv, front, head) - 20.0;
    float nasal = sdLine(uv, head, nose) - 15.0;
    float eye = sdCircle(uv, eye_p, 10.0);

    float ear = sdLine(uv, ears, ears_end) - 4.0;

    float d = smin(d1, d2, 50.0);
    d = smin(d, d3, 100.0);
    d = smin(d, backleg, 30.0);
    d = smin(d, frontleg, 30.0);
    d = smin(d, neck, 70.0);
    d = smin(d, nasal, 100.0);
    d = max(d, -eye);
    d = smin(d, tail, 40.0);
    d = smin(d, ear, 30.0);

    return d;
}

float sdParabola(in vec2 pos, in float k) {
    pos.x = abs(pos.x);

    float p = (1.0 - 2.0 * k * pos.y) / (6.0 * k * k);
    float q = -abs(pos.x) / (4.0 * k * k);

    float h = q * q + p * p * p;
    float r = sqrt(abs(h));

    float x = (h > 0.0) ? pow(-q + r, 1.0 / 3.0) - pow(abs(-q - r), 1.0 / 3.0) * sign(q + r) : 2.0 * cos(atan(r, -q) / 3.0) * sqrt(-p);

    return length(pos - vec2(x, k * x * x)) * sign(pos.x - x);
}

mat2 rotate(float theta) {
    return mat2(cos(theta), -sin(theta), sin(theta), cos(theta));
}

void main() {
    vec2 uv = ((gl_FragCoord.xy / resolution.y) * 2.0 - 1.0) * 500.0 * rotate(sin(time * 2.0) * 0.5) * (1.0 + sin(time * 9.0) * 0.3);
    float ct = time * 5.0;
    float d  = sdCircle(uv, vec2(0.0, -500.0), 400.0);

    d = min(d, horseDist(translate(uv, vec2(sin(time), 0.0) * 100.0)));
    d = smoothstep(1.5 * 1000.0 / resolution.y, 0.0, d - 10000.0 / exp(time * 10.0));

    outColor = vec4(vec3(0.5 + 0.5 * sin(ct), 0.5 + 0.5 * cos(ct * 1.3), 0.5 + 0.5 * cos(ct * 1.5)) * 0.5 + d, 1.0);
}
