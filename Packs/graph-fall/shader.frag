#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

void main() {
    vec2 uv = (gl_FragCoord.xy - resolution) / max(resolution.x, resolution.y);
    uv.x -= 0.25;
    uv.y += 0.25;
    uv *= 15.0;

    float e = 0.0;
    for (float i = 1.0; i <= 50.0; i += 1.0) {
        e += 0.005 / abs((i / 500.0) + tan((time / 3.0) + 0.05 * i * uv.x * (sin(i / 3.0 + time / 32.0 + uv.x * 0.5))) + 2.5 * uv.y);
        outColor = vec4(vec3(e / 20.0, e / 3.0, e / 1.6), 20.0);
    }
}
