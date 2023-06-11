#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;
uniform vec4 mouse;

#define GAMMA 2.2

#define MAX_STEPS 90
#define MAX_DIST 100.0
#define MIN_DIST 10.0

#define GLOW_INT 1.0
#define PP_ACES 1.0
#define PP_CONT 0.5
#define PP_VIGN 1.3
#define AO_OCC 0.5
#define AO_SCA 0.3

#define PI 3.14159265
#define TAU 6.28318531
#define S(x, y, t) smoothstep(x, y, t)
#define sin3(x) sin(x) * sin(x) * sin(x)
#define Rot2D(p, a) p = cos(a) * p + sin(a) * vec2(p.y, -p.x)

vec3 Rot(in vec3 p, in vec3 r) {
    Rot2D(p.xz, r.y);
    Rot2D(p.yx, r.z);
    Rot2D(p.zy, r.x);
    return p;
}

float sdKMC(in vec3 p, in int iters, in vec3 fTra, in vec3 fRot, in vec4 para) {
    int i = 0;
    float col = 0.0;
    float x1, y1;
    float r = p.x * p.x + p.y * p.y + p.z * p.z;
    
    for(int ii = 0; ii < 8; ii++) {
        if (ii >= iters) {
            break;
        }
        if (r >= 1e6) {
            break;
        }
        if (i > 0) {
            p -= fTra;
            p = Rot(p, fRot);
        }

        p = abs(p);

        if (p.x-p.y < 0.0) {
            x1 = p.y;
            p.y = p.x;
            p.x = x1;
        }
        if (p.x-p.z < 0.0) {
            x1 = p.z;
            p.z = p.x;
            p.x = x1;
        }
        if (p.y-p.z < 0.0) {
            y1 = p.z;
            p.z = p.y;
            p.y = y1;
        }

        p.z -= 0.5 * para.x * (para.y - 1.0) / para.y;
        p.z = -abs(p.z);
        p.z += 0.5 * para.x * (para.y - 1.0) / para.y;

        p.x = para.y * p.x - para.z * (para.y - 1.0);
        p.y = para.y * p.y - para.w * (para.y - 1.0);
        p.z = para.y * p.z;

        r = p.x * p.x + p.y * p.y + p.z * p.z;
        i++;
    }

    return length(p) * pow(para.y, float(-i));
}

vec2 SDF(vec3 p, float depth) {
    float d = MAX_DIST, col = 0.0;
        
    p = abs(Rot(p, vec3(10.5 - depth)));

    float sphere = length(p - vec3(1.8 + sin(time / 3.0 + depth) * 0.6, 0, 0)) - 0.1;
    col = mix(col, 1.7, step(sphere, d));
    d = min(sphere, d);
    
    float torus = length(vec2(length(p.yz) - 1.2, p.x)) - 0.01;
    col = mix(col, 1.3, step(torus, d));
    d = min(torus, d);
        
    float menger = sdKMC(p * 2.9, 8, vec3(sin(time / 53.0)) * 0.4, vec3(sin3(time / 64.0) * PI), vec4(2.0, 3.5, 4.5, 5.5)) / 2.9;
    col = mix(col, floor(mod(length(p) * 1.5, 4.0)) + 0.5, step(menger, d));
    d = min(menger, d);
    
    return vec2(d, col);
}

float dens = 0.9;

vec2 Map(in vec3 p) {
    vec3 pos = p;
    
    float r = length(p);
    p = vec3(log(r), acos(p.z / r), atan(p.y, p.x));

    float t = time / 10.0 + mouse.x / resolution.x * 3.0;
    p.x -= t;
    float scale = floor(p.x * dens) + t * dens;
    p.x = mod(p.x, 1.0 / dens);

    float erho = exp(p.x);
    float sintheta = sin(p.y);
    p = vec3(erho * sintheta * cos(p.z), erho * sintheta * sin(p.z), erho * cos(p.y));

    vec2 sdf = SDF(p, scale);
    sdf.x *= exp(scale / dens);

    return sdf;
}

vec3 Normal(in vec3 p, in float depth) {
    float h = depth / resolution.y;
    vec2 k = vec2(1, -1);
    return normalize(k.xyy * Map(p + k.xyy * h).x + k.yyx * Map(p + k.yyx * h).x + k.yxy * Map(p + k.yxy * h).x + k.xxx * Map(p + k.xxx * h).x);
}

vec3 RayMarch(vec3 ro, vec3 rd) {
    float col = 0.0;
	float dO = mix(MIN_DIST, MAX_DIST / 2.0, S(0.9, 1.0, sin(time / 24.0) * 0.5 + 0.5));
    int steps = 0;
    
    for(int i = 0; i < MAX_STEPS; i++) {
        steps = i;
        
    	vec3 p = ro + rd * dO;
        vec2 dS = Map(p);
        col = dS.y;
        dO += min(dS.x, length(p) / 12.0);
        
        if (dO > MAX_DIST || dS.x < dO / resolution.y) {
            break;
        }
    }
    
    return vec3(steps == 0 ? MIN_DIST : dO, steps, col);
}

float CalcAO(const in vec3 p, const in vec3 n) {
    float occ = AO_OCC;
    float sca = AO_SCA;

    for (int i = 0; i < 5; i++) {
        float h = 0.001 + 0.150 * float(i) / 4.0;
        float d = Map(p + h * n).x;
        occ += (h - d) * sca;
        sca *= 0.95;
    }
    return S(0.0, 1.0, 1.0 - 1.5 * occ);
}

const vec3 ambCol = vec3(0.03, 0.05, 0.1) * 5.5;
const vec3 sunCol = vec3(1.0, 0.7, 0.4) * 1.2;
const vec3 skyCol = vec3(0.3, 0.5, 1.0) * 0.04;
const float specExp = 4.0;

vec3 Shade(vec3 col, float mat, vec3 p, vec3 n, vec3 rd, vec3 lp) {
    vec3 lidi = normalize(lp - p);
    float amoc = CalcAO(p, n);
    float diff = max(dot(n, lidi), 0.0);
    float spec = pow(diff, max(1.0, specExp * mat));
    float refl = pow(max(0.0, dot(lidi, reflect(rd, n))), max(1.0, specExp * 3.0 * mat));

    return  col * (amoc * ambCol + (1.0 - mat) * diff * sunCol + mat * (spec + refl) * sunCol);
}

vec3 hsv2rgb_smooth(in vec3 c) {
    vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
	rgb = rgb * rgb * (3.0 - 2.0 * rgb);
	return c.z * mix(vec3(1.0), rgb, c.y);
}

vec3 Palette(int index)
{
         if (index == 0) return vec3(1., 1., 1.);
    else if (index == 1) return vec3(1., .8, .6);
    else if (index == 2) return vec3(.6, .8, 1.);
    else if (index == 3) return hsv2rgb_smooth(vec3(fract(time/21.), .65, .8));

    return vec3(0.);
}

vec3 Ray(in vec2 uv, in vec3 p, in vec3 l) {
    vec3 f = normalize(l - p);
    vec3 r = normalize(cross(vec3(0, 1, 0), f));
    vec3 u = cross(f, r);
    vec3 c = p + f;
    vec3 i = c + uv.x * r + uv.y * u;
    return normalize(i - p);
}

vec4 PP(vec3 col, vec2 uv) {
    col = mix(col, (col * (2.51 * col + 0.03)) / (col * (2.43 * col + 0.59) + 0.14), PP_ACES);
    col = mix(col, S(vec3(0), vec3(1), col), PP_CONT);
    col *= S(PP_VIGN, -PP_VIGN / 5.0, dot(uv, uv));
    col = pow(col, vec3(1) / GAMMA);
    
    return vec4(col, 1.0);
}

void main() {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    vec2 m = mouse.xy / resolution.xy;
    if (length(m) <= 0.1) {
        m = vec2(0.5);
    }

    vec3 ro = vec3(0, 0, -MAX_DIST / 2.0);
    ro.yz = Rot2D(ro.yz, -m.y * PI + PI * 0.5);
    ro.xz = Rot2D(ro.xz, -m.x * PI * 2.0 - PI);
    vec3 rd = Ray(uv, ro, vec3(0));

    vec3 bg = skyCol;
    vec3 col = bg;
    vec3 p = vec3(0);
    vec3 rmd = RayMarch(ro, rd);

    if (rmd.x <= MIN_DIST) {
     col = Palette(int(floor(rmd.z))) / 8.0;
    } else if (rmd.x < MAX_DIST) {
        p = ro + rd * rmd.x;
        vec3 n = Normal(p, rmd.x);

        float shine = fract(rmd.z);
        col = Palette(int(floor(abs(rmd.z))));
        col = Shade(col, shine, p, n, rd, vec3(0));
    }

    float disFac = S(0.0, 1.0, pow(rmd.x / MAX_DIST, 2.0));

    col = mix(col, bg, disFac);
    col += pow(rmd.y / float(MAX_STEPS), 2.5) * normalize(ambCol) * (GLOW_INT + (rmd.x < MAX_DIST ? 3.0 * S(0.995, 1.0, sin(time / 2.0 - length(p) / 20.0)) : 0.0));

    outColor = PP(col, uv);
}
