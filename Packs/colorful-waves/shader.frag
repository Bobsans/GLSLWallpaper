#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

#define SPEED 50.0

void main() {
    vec2 p = gl_FragCoord.xy * 20.0 / max(resolution.x, resolution.y);

    for (float i = 1.0; i < 100.0; i++) {
        vec2 newp = p;
        newp.x += 0.6 / i * sin(i * p.y + time / (100.0 / SPEED) + 0.3 * i) + 1.0 + (mouse.x / resolution.x * 0.05);
        newp.y += 0.6 / i * sin(i * p.x + time / (100.0 / SPEED) + 0.3 * i) - 1.0 + (mouse.y / resolution.y * 0.05);
        p = newp;
    }

    outColor = vec4(sin(p.x) / 2.0 + 0.5, sin(p.x + p.y) / 3.0 + 0.3, 0.4, 5.0);
}
