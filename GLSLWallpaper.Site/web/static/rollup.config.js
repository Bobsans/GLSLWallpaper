import scss from 'rollup-plugin-scss';
import nodeResolve from '@rollup/plugin-node-resolve';
import typescript from '@rollup/plugin-typescript';

export default {
    input: 'src/index.ts',
    output: {
        file: 'dist/bundle.js',
        format: 'iife'
    },
    plugins: [
        nodeResolve({extensions: ['.js', '.ts']}),
        typescript(),
        scss({fileName: 'bundle.css'})
    ]
}
