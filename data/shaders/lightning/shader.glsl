#version 120

#ifdef GL_ES
precision lowp float;
#endif

uniform float time;
uniform vec2 resolution;

const float count = 2.0;
const float speed = 0.3;

float Hash(vec2 p, in float s) {
    return fract(sin(dot(vec3(p.xy, 27.0 * abs(sin(s))), vec3(27.1, 61.7, 12.4))) * 2.1);
}

float noise(in vec2 p, in float s) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f *= f * (3.0 - 2.0 * f);
   
    return mix(mix(Hash(i + vec2(0.0, 0.0), s), Hash(i + vec2(1.0, 0.0), s), f.x), mix(Hash(i + vec2(0.0, 1.0), s), Hash(i + vec2(1.0, 1.0), s), f.x), f.y) * s;
}

float fbm(vec2 p) {
    float v = -6.00;
    v += noise(p * 1.0, 0.40);
    v += noise(p * 2.0, 0.27);
    v += noise(p * 4.0, 0.10);
    v += noise(p * 8.0, 0.05);
    return v;
}

float rand(float n) {
    return fract(sin(n) * 43758.5453123);
}

float noise3(float p) {
    float fl = floor(p);
    float fc = fract(p);
    return mix(rand(fl), rand(fl + 1.0), fc);
}

float rand2(vec2 n) { 
    return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise2(vec2 p) {
    vec2 ip = floor(p);
    vec2 u = fract(p);
    u = u * u * (3.0 - 2.0 * u);
    
    float res = mix(mix(rand2(ip), rand2(ip + vec2(1.0, 0.0)), u.x), mix(rand2(ip + vec2(0.0, 1.0)), rand2(ip + vec2(1.0, 1.0)), u.x), u.y);
    return res * res;
}

void main(void) {
    float worktime = time * speed;
    float myTime = fract(time) * 0.01;
    float flowTime = abs(0.5 - mod(time, 1.0)) * 0.1;
    vec2 uv = (gl_FragCoord.xy / resolution.xy) * 8.0;
    uv.x *= resolution.x / resolution.y;
    uv.x *= 0.7;
    
    vec3 finalColor = vec3(0.0);
    vec3 color = vec3(1.0, 1.0, 1.0);
    float a = 0.0;
    
    float veryNoise = noise(uv, 0.5) * 3.0;
    float veryNoise1 = noise3(worktime) * 5.0;
    float veryNoise2 = noise2(uv * 1.5) * 2.0;
    
    float t1 = uv.y + fbm(uv + worktime) + veryNoise;
    float t2 = uv.y + fbm(uv + worktime / 2.0) + veryNoise;
        
    float linia1 = abs(0.3 / (t1 * 50.0));
    linia1 *= 3.0;
    linia1 = smoothstep(0.3, 0.5, linia1);
    a += linia1;
    
    for(float i = 2.0; i <= count; ++i) {   
        float t = abs(0.3 / (((uv.y + fbm(uv + worktime / i)) + veryNoise) * (i * 50.0)));
        t *= i * 0.8;
        t = smoothstep(0.1, 0.5, t);        
    
        if (t2 < t1) {
            a += t;
        }
    }
    
    color *= a;
    
    color.r = clamp(color.r, 0.0, 0.18);
    color.g = clamp(color.g, 0.0, 0.71);
    color.b = clamp(color.b, 0.0, 0.95);
    
    gl_FragColor = vec4(color, 1.0);
}