#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

uniform sampler2D u_noise;

const float AA = 0.02;
const float blur = 0.2;
const float size = 0.3;
const float width = 0.08;

#define PI 3.14159265358979323846

vec2 diagonalhash2(vec2 p) {
    return fract(vec2(sin((p.x + p.y) * 15.543) * 73964.686, sin((p.x + p.y) * 55.8543) * 28560.986));
}

float rand(vec2 c) {
    return fract(sin(dot(c.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

float noise(vec2 p, float freq) {
    float unit = resolution.x / freq;
    vec2 ij = floor(p / unit);
    vec2 xy = mod(p, unit) / unit;
    xy = 0.5 * (1.0 - cos(PI * xy));
    float a = rand((ij + vec2(0.0, 0.0)));
    float b = rand((ij + vec2(1.0, 0.0)));
    float c = rand((ij + vec2(0.0, 1.0)));
    float d = rand((ij + vec2(1.0, 1.0)));
    float x1 = mix(a, b, xy.x);
    float x2 = mix(c, d, xy.x);
    return mix(x1, x2, xy.y);
}

vec3 pattern(vec2 uv, vec2 m) {
    vec2 grid = floor(uv);
    vec2 subuv = fract(uv);
    float seed = noise(grid, 1000.0);
    float phase = sin(time * 0.5 + seed * 10.0);
    float shape = 0.0;
    float df;
    vec3 col = vec3(0.6, 0.8, 0.3);

    if (seed < 0.5) {
        df = length(subuv - 0.5);
    } else {
        float s = sin(0.785398);
        float c = cos(0.785398);
        subuv = (subuv - 0.5) * mat2(c, -s, s, c);
        vec2 offsetuv = (abs(subuv) + vec2(0.0, 0.3));
        df = max(offsetuv.x, offsetuv.y);
        offsetuv = abs(subuv) + vec2(0.3, 0.0);
        df = min(df, max(offsetuv.x, offsetuv.y));
        col = vec3(0.9, 0.3, 0.2);
    }

    float w = width * max(phase, 0.1);

    shape = (smoothstep(size + w + AA, size + w, df) - smoothstep(size - w + AA, size - w, df)) * phase;
    shape += (smoothstep(size + w * 0.1 + blur, size + w * 0.1, df) - smoothstep(size - w * 0.1, size - w * 0.1 - blur, df)) * phase;

    vec3 colour = vec3(col * shape * 2.0);

    return colour;
}

vec3 layer(inout vec2 uv, in float toffset, in float multiplier, in mat2 rot, in vec2 m) {
    uv *= multiplier;
    uv -= toffset;
    uv *= rot;
    return pattern(uv, m);
}

vec2 getScreenSpace() {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution.xy) / min(resolution.y, resolution.x);
    float l = length(uv);

    uv *= 1.0 + dot(l, l) * 0.2;
    uv *= 4.0 + sin(time * 0.1) * 3.0;

    vec2 dir = vec2(time * 0.35, sin(time * 0.1) * 0.8);
    float a = dir.y * - 0.2;
    float c = cos(a);
    float s = sin(a);

    uv *= mat2(c, -s, s, c);
    uv += dir;

    return uv;
}

void main() {
    vec2 uv = getScreenSpace();

    outColor = texture(u_noise, uv + diagonalhash2(uv + time * 0.1)) * 0.2;

    vec2 m = mouse.xy - uv;
    float multiplier = 1.5;
    float toffset = time * 0.1;
    float a = 0.5;
    float c = cos(a);
    float s = sin(a);
    mat2 rot = mat2(c, -s, s, c);

    vec3 colour = pattern(uv, m) * 2.0;
    vec3 ocolour = colour;

    colour += layer(uv, toffset, multiplier, rot, m) * 0.25 + 0.025;
    colour += layer(uv, toffset, multiplier, rot, m) * 0.5 + 0.25;
    colour *= 0.5;
    colour += layer(uv, toffset, multiplier, rot, m) * 0.5 + 0.25;
    colour += layer(uv, toffset, multiplier, rot, m) * 0.25 + 0.125;
    colour += layer(uv, toffset, multiplier, rot, m) * 0.25 + 0.125;

    outColor += vec4(colour * 0.3, 1.0);
}
