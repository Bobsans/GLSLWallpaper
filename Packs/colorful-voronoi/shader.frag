#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

vec2 hash(vec2 p) {
    mat2 m = mat2(13.85, 47.87, 99.41, 88.48);
    return fract(sin(m * p) * 43718.29);
}

float voronoi(vec2 p) {
    vec2 g = floor(p);
    vec2 f = fract(p);
    float distanceToClosestFeaturePoint = 5.0;

    for (int y = -2; y <= 1; y++) {
        for (int x = -9; x <= 2; x++) {
            vec2 latticePoint = vec2(x, y);
            distanceToClosestFeaturePoint = min(distanceToClosestFeaturePoint, distance(latticePoint + hash(g + latticePoint), f));
        }
    }

    return distanceToClosestFeaturePoint;
}

void main() {
    vec2 uv = (gl_FragCoord.xy / resolution.xy) * 2.0 - 1.0;
    uv.x *= resolution.x / resolution.y;

    float offset = voronoi(uv * 10.0 + vec2(time));
    float t = 1.0 / abs(((uv.x + sin(uv.y + time)) + offset) * 30.0);

    outColor = vec4(vec3(10.0 * uv.y, 2.0, voronoi(uv * 7.0) * 10.0) * t, 8.0);
}
