#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

void main() {
    vec3 col = vec3(0);
    const int AA = 2;
    vec2 pos = gl_FragCoord.xy;
    vec2 h = resolution * 0.5;
    for (int j = 0; j < AA; j++) {
        for (int i = 0; i < AA; i++) {
            vec2 p = (pos + vec2(i, j) / float(AA) - h) / resolution.y;
            float ttm = cos(sin(time / 8.0)) * 6.21;
            p *= mat2(cos(ttm), sin(ttm), -sin(ttm), cos(ttm));
            p -= vec2(cos(time / 2.0) / 2.0, sin(time / 3.987) / 5.99);
            float zm = (200.0 + sin(time / 7.0) * 50.0);
            vec2 cc = vec2(-0.57735 + 0.004 - 0.01, 0.57735) + p / zm;
            vec2 z = vec2(0), dz = vec2(0);
            const int iter = 128;
            int ik = 128;
            vec3 fog = vec3(0);
            for (int k = 0; k < iter; k++) {
                dz = mat2(z, -z.y, z.x) * dz * 2.0 + vec2(1, 0);
                z = mat2(z, -z.y, z.x) * z + cc;
                if (dot(z, z) > 200.0) {
                    ik = k;
                    break;
                }
            }
            float ln = step(0.0, length(z) / 15.5 - 1.0);
            float d = sqrt(1.0 / max(length(dz), 0.0001)) * log(dot(z, z));
            d = clamp(d * 50.0, 0.0, 1.0);
            float dir = mod(float(ik), 2.0) < 0.5 ? -1.0 : 1.0;
            float sh = (float(iter - ik)) / float(iter);
            vec2 tuv = z / 320.0;
            float tm = -ttm * sh * sh * 16.0;
            tuv *= mat2(cos(tm), sin(tm), -sin(tm), cos(tm));
            tuv = abs(mod(tuv, 0.125) - 0.0625);
            float pat = smoothstep(0.0, 1.0 / length(dz), length(tuv) - 0.03125);
            pat = min(pat, smoothstep(0.0, 1.0 / length(dz), abs(max(tuv.x, tuv.y) - 0.0625) - 0.0025));
            vec3 lCol = pow(min(vec3(1.5, 1, 1) * min(d * 0.85, 0.96), 1.0), vec3(1, 3, 16)) * 1.15;
            lCol = dir < 0.0 ? lCol * min(pat, ln) : (sqrt(lCol) * 0.5 + 0.7) * max(1.0 - pat, 1.0 - ln);
            vec3 rd = normalize(vec3(p, 1.0));
            rd = reflect(rd, vec3(0, 0, -1));
            float diff = clamp(dot(z * 0.5 + 0.5, rd.xy), 0.0, 1.0) * d;
            tuv = z / 200.0;
            tm = -tm / 1.5 + 0.5;
            tuv *= mat2(cos(tm), sin(tm), -sin(tm), cos(tm));
            tuv = abs(mod(tuv, 0.125) - 0.0625);
            pat = smoothstep(0.0, 1.0 / length(dz), length(tuv) - 0.03125);
            pat = min(pat, smoothstep(0.0, 1.0 / length(dz), abs(max(tuv.x, tuv.y) - 0.0625) - 0.0025));
            lCol += mix(lCol, vec3(1) * ln, 0.5) * diff * diff * 0.5 * (pat * 0.6 + 0.6);
            if (mod(float(ik), 6.0) < 0.5) {
                lCol = lCol.yxz;
            }
            lCol = mix(lCol.xzy, lCol, d / 1.2);
            lCol = mix(lCol, vec3(0), (1.0 - step(0.0, -(length(z) * 0.05 * float(ik) / float(iter) - 1.0))) * 0.95);
            lCol = mix(fog, lCol, sh * d);
            col += 1.0 - (1.0 - min(lCol, 1.0));
        }
    }
    col /= float(AA * AA);
    vec2 uv = pos / resolution;
    col *= pow(abs(16.0 * (1.0 - uv.x) * (1.0 - uv.y) * uv.x * uv.y), 0.125) * 1.15;
    outColor = vec4(sqrt(max(col, 0.0)), 1.0);
}
