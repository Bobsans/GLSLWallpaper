#version 300 es
precision highp float;

out vec4 outColor;

uniform float time;
uniform vec2 resolution;

mat2 inverse(mat2 m) {
   mat2 adj;
   adj[0][0] = m[1][1];
   adj[0][1] = -m[0][1];
   adj[1][0] = -m[1][0];
   adj[1][1] = m[0][0];
   float det = m[0][0] * m[1][1] - m[1][0] * m[0][1];
   return adj / det;
}


// Simple Hex, Tri and Square grids (SST)
//
// Feel free to optimize, golf and generally improve them :)
//
// Del - 15/03/2021
vec4 HexGrid(vec2 uv, out vec2 id)
{
    uv *= mat2(1.1547,0.0,-0.5773503,1.0);
    vec2 f = fract(uv);
    float triid = 1.0;
	if((f.x+f.y) > 1.0)
    {
        f = 1.0 - f;
     	triid = -1.0;
    }
    vec2 co = step(f.yx,f) * step(1.0-f.x-f.y,max(f.x,f.y));
    id = floor(uv) + (triid < 0.0 ? 1.0 - co : co);
    co = (f - co) * triid * mat2(0.866026,0.0,0.5,1.0);    
    uv = abs(co);
    return vec4(0.5-max(uv.y,abs(dot(vec2(0.866026,0.5),uv))),length(co),co);
}

// EquilateralTriangle distance
float sdEqTri(in vec2 p)
{
    const float k = 1.7320508;//sqrt(3.0);
    p.x = abs(p.x) - 0.5;
    p.y = p.y + 0.5/k;
    if( p.x+k*p.y>0.0 ) p = vec2(p.x-k*p.y,-k*p.x-p.y)/2.0;
    p.x -= clamp( p.x, -1.0, 0.0 );
    return -length(p)*sign(p.y);
}

vec4 TriGrid(vec2 uv, out vec2 id)
{
    const vec2 s = vec2(1, .8660254); // Sqrt (3)/2
    uv /= s;
    float ys = mod(floor(uv.y), 2.)*.5;
    vec4 ipY = vec4(ys, 0, ys + .5, 0);
    vec4 ip4 = floor(uv.xyxy + ipY) - ipY + .5; 
    vec4 p4 = fract(uv.xyxy - ipY) - .5;
    float itri = (abs(p4.x)*2. + p4.y<.5)? 1. : -1.;
    p4 = itri>0.5? vec4(p4.xy*s, ip4.xy) : vec4(p4.zw*s, ip4.zw);  

    vec2 ep = p4.xy;
    ep.y = (ep.y + 0.14433766666667*itri) * itri;
    float edge = sdEqTri(ep); // dist to edge
    id = p4.zw;
    //id *= mat2(1.1547,0.0,-0.5773503,1.0); // adjust ID (optional)
    p4.y+=0.14433766666667*itri;
    return vec4(abs(edge),length(p4.xy),p4.xy);
}

vec4 SquareGrid(vec2 uv, out vec2 id)
{
    vec2 fs =  fract(uv)-0.5;
    id = floor(uv);
    //id *= mat2(1.1547,0.0,-0.5773503,1.0); // adjust ID (optional)
    vec2 d = abs(fs)-0.5;
    float edge = length(max(d,0.0)) + min(max(d.x,d.y),0.0);
    return vec4(abs(edge),length(fs),fs.xy);
}
mat2 rot(float a)
{
    float s=sin(a),c=cos(a);
    return mat2(c,s,-s,c);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{

    vec2 uv = (fragCoord.xy - 0.5 * resolution.xy) / resolution.y;
	uv.yx /= sin(time*0.8+uv.y)+1.8;
	uv*=rot(time*0.1+sin(length(uv)+time*0.4)*1.5);
	uv *= sin(0.25+length(uv*0.5));
	
    vec3 color = vec3(0.3, 0.44, 0.2);
    vec2 id;
    vec4 h = TriGrid(uv*26.0, id);
	
    vec2 trid = id*0.15;
    float cm = sin(time*4.0+length(trid*2.5)*4.0);
    cm = clamp(cm, 0.0, 1.);

    vec3 bordercol = vec3(0.5,0.6,0.4);
    vec3 finalcol = mix(bordercol,color*cm, smoothstep(0.0,0.035,h.x));
	
    fragColor = vec4(finalcol,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(outColor, gl_FragCoord.xy);
}
