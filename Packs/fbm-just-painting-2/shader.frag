#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

#define pi acos(-1.0)

float noise(vec2 st) {
    return fract(sin(dot(vec2(12.23, 74.343), st)) * 43254.0);
}

float rnd(float a) {
    return fract(sin(a * 234.15) * 41240.91);
}

float noise2D(vec2 st) {
    vec2 id = floor(st);
    vec2 f = fract(st);
    float a = noise(id);
    float b = noise(id + vec2(1.0, 0.0));
    float c = noise(id + vec2(0.0, 1.0));
    float d = noise(id + vec2(1.0));

    f = smoothstep(0.0, 1.0, f);

    float ab = mix(a, b, f.x);
    float cd = mix(c, d, f.x);
    return mix(ab, cd, f.y);
}

mat2 rot45 = mat2(0.707, -0.707, 0.707, 0.707);

mat2 rot(float a) {
    float s = sin(a);
    float c = cos(a);
    return mat2(c, -s, s, c);
}

float fbm(vec2 st, float N, float rt) {
    st *= 3.0;
 
    float s = 0.5;
    float ret = 0.0;
    for (float i = 0.0; i < 5.0; i++) {
        ret += noise2D(st) * s;
        st *= 2.9;
        s /= 2.0;
        st *= rot((pi * (i + 1.0) / N) + rt * 8.0);
        //st.x += time;
    }
    return ret;
}

void main() {
    vec2 uv = (gl_FragCoord.xy - resolution.xy * 0.5) / resolution.y;
    vec3 col = vec3(0.0);

    vec3 theColor;

    for (float i = 1.0; i < 14.0; i++) {
        vec2 altUV = uv;
        float j = pow(rnd(i) * 0.6, 0.5);
        theColor = vec3(j, j * j * j, j * j * 0.27);
        uv = uv + vec2(time / (80.0 - i * 2.0), 0.0);
        float faa = fbm(uv + i * 100.0, 5.0, 6.0);
        faa = abs(faa);
        float shadow = smoothstep(0.5, 0.3, faa);
        col = mix(col, theColor * 0.1, shadow * 0.5);

        //uv = uv + vec2(time/(80.+i*8.),0.);
        float faa2 = fbm(uv + i * 100.0 + 0.03, 5.0, 6.0);
        float shadow2 = clamp(smoothstep(0.4, 0.36, faa2), 0.0, 1.0);
        col = mix(col, vec3(0.0), shadow2 * 0.7);

        float stripes = smoothstep(0.3, 0.2, abs(fract((uv.y - uv.x) * (15.0 + i * 1.3)) - 0.5));

        float f = max(0.0, smoothstep(0.4, 0.39, faa));
        float g = max(0.0, smoothstep(0.4, 0.39, faa));
        float h = clamp(g - max(0.0, smoothstep(0.36, 0.34, faa)), 0.0, 1.0);

        if (mod(i, 3.0) == 0.0) {
            stripes = 1.0;
            theColor = theColor.bgg;
        }

        col = mix(col, theColor - stripes * 0.04, f);
        col = mix(col, theColor * 0.9, h);
    }

    col *= 1.3;
    uv = gl_FragCoord.xy / resolution.xy;
    uv *= 1.0 - uv.yx;
    float vig = uv.x * uv.y * 25.0;
    vig = pow(vig, 0.3);

    outColor = vec4(col * vig * 1.4, 1.0);
}
