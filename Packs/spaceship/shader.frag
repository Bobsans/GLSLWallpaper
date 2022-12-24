precision mediump float;

uniform vec2 resolution;
uniform float time;

const float pi = acos(-1.);
const float pi2 = pi*2.;

const float bpm = 138.;

mat2 rot(float r)
{
  return mat2(cos(r),sin(r),-sin(r),cos(r));
}

vec2 pmod(vec2 p, float r)
{
  float a = atan(p.x,p.y) + pi/r;
  float n = pi2/r;
  a = floor(a/n)*n;
  return p*rot(-a);
}

float smoothMin(float d, float d2, float k)
{
  float a = exp(-d*k)+exp(-d2*k);
  return -log(a)/k;
}
float sphere(vec3 p, float r)
{
  return length(p) - r;
}
float cube(vec3 p, vec3 s)
{
  p = abs(p)-s;
  return max(max(p.x,p.y),p.z);
}

float center(vec3 p)
{
  float d = cube(p,vec3(1.));
  float d2 = cube(p,vec3(.3,3.,.3));
  float d3 = cube(p,vec3(3.,.3,.3));
  vec3 q = p;
  p.xy = abs(q.xy) - .6 ;
  float d4 = cube(p,vec3(.3,.3,3.));

  float obj = max(min(min(d,d2),d3),-d4);

  return obj;
}

float center2(vec3 p)
{
  p += vec3(0.,0.,10.);
  float d = cube(p,vec3(.2,.1,2.5));
  vec3 q = p;
  q.xz *= rot(pi*0.25);
  float d2 = cube(q-vec3(0.6,0.,0.6),vec3(1.3,0.1,1.3));
  vec3 w = p;
  w.yz *= rot(pi*0.25);
  float d3 = cube(w-vec3(0.,1.5,1.),vec3(.1,.7,.7));
  vec3 r = p;
  float d4 = cube(r-vec3(0.,0.,-1.5),vec3(.1,.3,1.));

  return smoothMin(smoothMin(smoothMin(d,d2,5.),d4,8.),d3,5.);
}

float center2move(vec3 p)
{
  p.y += 4.*sin(time);
  p.xy *= rot(time+4.*exp(sin(time)));
  p.z += 8.*exp(sin(time));
  float d = center2(p-vec3(.75,0.,0.));
  float d2 = center2(p+vec3(.75,0.,0.));

  return min(d,d2);
}

float map(vec3 p)
{
  //bpm
  float tbpm = time * bpm / 60.;
  float seq = floor(tbpm);

  p.xy *= rot(-time*.115);
  p.z -= 20.*tbpm-5.*exp(sin(pi2*tbpm*0.115));
  float obj = center(p);
  float add = 1.;
  vec3 scale  = vec3(1.,1.,1.);
  for(float i = 0.; i < 5.; i++)
  {
    vec3 q = p;
    q.xy *= rot(q.z*0.02);
    q.z = mod(q.z,5.)-2.5;
    q.x = abs(q.x) - add;
    q.y = abs(q.y) - add;
    obj = max(center(q),-center(p));
    p = q;
    add = scale.x * 0.8;
    scale += 1.;
  }

  return obj;
}

void main()
{
  vec2 p = (gl_FragCoord.xy * 2. - resolution.xy) / min(resolution.x,resolution.y);

  vec3 cp = vec3(0.,0.,10.);
  cp.xz *= rot(sin(time));
  cp.yz *= rot(cos(time));
  vec3 cd = vec3(0.,0.,-1.);
  vec3 cu = vec3(0.,1.,0.);
  vec3 cs = cross(cd,cu);
  float td = 1.5;

  vec3 ray = normalize(p.x * cs + p.y * cu + cd * td);
  vec3 col = vec3(0.);
  float mainEmissive = 0.;
  float subEmissive = 0.;
  float fog = 0.;

  float d,rl = 0.;
  float d2 = 0.;
  vec3 rp = cp;

  for(int i = 0; i < 128; i ++)
  {
    d = map(rp);
    d2 = center2move(rp);
    if(d<d2) mainEmissive += exp(d*-0.2);
    if(d>d2) subEmissive += exp(d2*-0.2);
    d=min(d,d2);
    if(d < 0.001)
    {
      break;
    }
    fog += d;
    rl += d;
    rp = cp + rl*ray;
  }
  col = vec3(1.,1.,1.);
  col *= mainEmissive*0.005;
  col += vec3(.5,.1,.1)*subEmissive*0.05;

  gl_FragColor = vec4(col,1.);
}
