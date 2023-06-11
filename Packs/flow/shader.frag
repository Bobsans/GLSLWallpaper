#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

const int complexity = 30;
const float fluid_speed = 3.0;
const float color_intensity = 0.21;

void main() {
    vec2 p = (2.0 * gl_FragCoord.xy - resolution) / max(resolution.x, resolution.y);

    for(int i = 1; i < complexity; i++) {
        vec2 newp = p;
        newp.x += 0.5 / float(i) * sin(float(i) * p.y + time / fluid_speed + 0.9 * float(i)) + 100.0;
        newp.y += 0.5 / float(i) * sin(float(i) * p.x + time / fluid_speed + 0.5 * float(i + 10)) - 100.0;
        p = newp;
    }

    vec3 col = vec3(color_intensity * sin(3.0 * p.x) + color_intensity + 0.4, color_intensity * sin(3.0 * p.y) + color_intensity + 0.3, sin(p.x + p.y) + 2.0);

    outColor = vec4(col, 1.0);
}
