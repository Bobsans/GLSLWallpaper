#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

void main() {
    float t = time * 0.6;
    vec2 o = gl_FragCoord.xy - resolution / 2.0;
    o = vec2(length(o) / resolution.y - 0.3, atan(o.y, o.x));
    vec4 s = 0.08 * cos(1.5 * vec4(0, 1, 2, 3) + t + o.y + sin(o.y) * cos(t));
    vec4 e = s.yzwx;
    vec4 f = max(o.x - s, e - o.x);

    outColor = dot(clamp(f * resolution.y, 0.0, 1.0), 72.0 * (s - e)) * (s - 0.1) + f;
}
