const shader = `
precision mediump float;

uniform float time;
uniform vec4 mouse;
uniform vec2 resolution;

vec3 palette( float t ) {
    vec3 a = vec3(0.5, 0.5, 0.5);
    vec3 b = vec3(0.5, 0.5, 0.5);
    vec3 c = vec3(1.0, 1.0, 1.0);
    vec3 d = vec3(0.263,0.416,0.557);

    return a + b*cos( 6.28318*(c*t+d) );
}

void main( void ) {
	
	float t = time*0.1;
	float t2 = time*4.0;

	
	vec2 position = ( gl_FragCoord.xy / resolution.xy) - 0.5;
	position.x += sin(time+position.x*2.0)*0.05;
	position.x *= resolution.x/resolution.y;
	position *= 1.75;
	position.y *= dot(position,position);
	
	position.y *= 1.0+sin(position.x*5.40+t2)*0.2;
	
	float foff = .33;
	float den = 0.07;
	float amp = .2;
	float freq = mouse.x*5. + 3.;
	float offset = 0.3-sin(position.x*0.5)*0.05;


	vec3 colour = vec3 (0.11, 0.22, 0.11) * 
		 ((1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq)))) * den)
		+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq+foff)))) * den)
		+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq-foff)))) * den));
	#define col colour
	
	col = 1. - exp( -col*mouse.y*6. );  // some cheap tonemapping
	col = palette( cos( col.y/2.5 + 1.42 ) );
	gl_FragColor = vec4( colour, 1.0 );
}`;

export default class Background {
    canvas: HTMLCanvasElement;
    gl: WebGLRenderingContext;
    program: WebGLProgram;
    vbo: WebGLBuffer;

    uTime: WebGLUniformLocation;
    uMouse: WebGLUniformLocation;
    uResolution: WebGLUniformLocation;
    aPosition: number;

    vertexPosition: number;

    _mouse: [number, number] = [0, 0];
    _resolution: [number, number] = [0, 0];

    constructor(canvas: HTMLCanvasElement) {
        this.canvas = canvas;

        this.onResize();

        const vShader = this.gl.createShader(this.gl.VERTEX_SHADER);
        this.gl.shaderSource(vShader, 'attribute vec4 position; void main() { gl_Position = position; }');
        this.gl.compileShader(vShader);

        if (!this.gl.getShaderParameter(vShader, this.gl.COMPILE_STATUS)) {
            console.warn(this.gl.getShaderInfoLog(vShader));
            this.cleanup();
            return;
        }

        const fShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.gl.shaderSource(fShader, shader);
        this.gl.compileShader(fShader);

        if (!this.gl.getShaderParameter(fShader, this.gl.COMPILE_STATUS)) {
            console.warn(this.gl.getShaderInfoLog(fShader));
            this.cleanup();
            return;
        }

        this.program = this.gl.createProgram();

        this.gl.attachShader(this.program, vShader);
        this.gl.attachShader(this.program, fShader);
        this.gl.linkProgram(this.program);
        this.gl.detachShader(this.program, vShader);
        this.gl.detachShader(this.program, fShader);
        this.gl.deleteShader(vShader);
        this.gl.deleteShader(fShader);

        if (!this.gl.getProgramParameter(this.program, this.gl.LINK_STATUS)) {
            console.warn(this.gl.getProgramInfoLog(this.program));
            this.cleanup();
            return;
        }

        this.uTime = this.gl.getUniformLocation(this.program, 'time');
        this.uMouse = this.gl.getUniformLocation(this.program, 'mouse');
        this.uResolution = this.gl.getUniformLocation(this.program, 'resolution');

        this.gl.useProgram(this.program);

        this.vbo = this.gl.createBuffer();
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vbo);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array([-1, -1, -1, 1, 1, 1, -1, -1, 1, 1, 1, -1]), this.gl.STATIC_DRAW);
        this.aPosition = this.gl.getAttribLocation(this.program, 'position');
        this.gl.vertexAttribPointer(this.aPosition, 2, this.gl.FLOAT, false, 0, 0);
        this.gl.enableVertexAttribArray(this.aPosition);

        requestAnimationFrame((dt) => this.updateLoop(dt));

        document.addEventListener('mousemove', (e) => {
            this._mouse = [e.clientX, e.clientY];
        });
    }

    public cleanup() {
        this.gl.useProgram(null);

        if (this.vbo) {
            this.gl.deleteBuffer(this.vbo);
        }

        if (this.program) {
            this.gl.deleteProgram(this.program);
        }
    }

    public onResize() {
        const bounds = this.canvas.getBoundingClientRect();
        this.canvas.width = bounds.width;
        this.canvas.height = bounds.height;
        this.gl = this.canvas.getContext('webgl', {
            antialias: false,
            depth: false,
            stencil: false,
            premultipliedAlpha: false,
            preserveDrawingBuffer: true
        });
        this.gl.getExtension('OES_standard_derivatives')

        this._resolution = [bounds.width, bounds.height];
    }

    public updateLoop(dt: number) {
        this.gl.uniform2f(this.uResolution, this._resolution[0], this._resolution[1]);
        this.gl.uniform1f(this.uTime, dt / 1000);
        this.gl.uniform4f(this.uMouse, this._mouse[0] / this._resolution[0], this._mouse[1] / this._resolution[1], 0, 0);

        this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);
        this.gl.drawArrays(this.gl.TRIANGLES, 0, 6);

        requestAnimationFrame((dt) => this.updateLoop(dt));
    }
}
