#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms
uniform float time;
uniform vec2 resolution;

// shadertoy emulation
#define iTime time
#define iResolution resolution

// --------[ Original ShaderToy begins here ]---------- //
mat2 rot(float a) {
    float s = sin(a), c = cos(a);
    return mat2(c, -s, s, c);
}

float rand(vec2 p) {
    return fract(sin(dot(p, vec2(3213.3123,4382.3123)))*4893423.432432);
}

float noise(float p) {
    float i = floor(p);
    float f = fract(p);
    return mix(rand(vec2(i)), rand(vec2(i+1.0)), f);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 p = (2.0*fragCoord-iResolution.xy)/iResolution.y;
    float pi = acos(-1.0);
    
    vec2 rp = fract(p*2.9)-0.5;
    vec2 id = floor(p*2.9);
    
    float sp = 1.5;
    float ph = mod(iTime*sp, (pi*2.0));
    float cy = floor(iTime*sp/(pi*2.0));
    
    float dir = floor(rand(id+vec2(cy*0.149))*114.0-57.0);
    float numr = floor(rand(id+vec2(cy*1.349))*10.0)*5.0;
    float ease = smoothstep(0.00001, 0.99999, 1.0/(1.0+exp(-7.0*sin(ph))));
    float theta = pi/4.0*( mod(dir, 8.0)-4.0 + 0.0 + numr)*(ease*2.0-1.0);
    theta *= mod(dir, 8.0)>4.0 ? 1.0 : -1.0;
    rp += vec2((noise(iTime*2.0+id.x)-0.5)*(0.5-abs(ease-0.5)),
               (noise(iTime*2.0+id.y)-0.5)*(0.5-abs(ease-0.5)));
    rp *= rot(theta);
    
    vec3 col = vec3(0.1);
    vec3 c = mix(vec3(1.0, 1.0, 1.0), vec3(1.0, 0.0, 0.0), 2.0*abs(ease-0.5)*step(0.7, rand(id+vec2(sin(cy*2.31)))));
    
    vec2 q = rp;
    col += smoothstep(0.0, 0.05, min(-abs(q.y)+0.3, -abs(q.x)+0.07))*c;
    
    q = rp;
    col += smoothstep(0.0, 0.05, min(q.y-0.2, -(q.y+abs(q.x)*1.2-0.5)))*c;

    col += (rand(p)*2.0-1.0)*0.1;
    col = min(col, 1.0);
    col -= length(p)*0.3;
    
    // Output to screen
    fragColor = vec4(col,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}