import path from "path";
import { defineConfig, loadEnv } from "vite";
import vue from "@vitejs/plugin-vue";
import { visualizer } from "rollup-plugin-visualizer";
import Unocss from "unocss/vite";
import {
    presetAttributify,
    presetIcons,
    presetUno,
    transformerDirectives,
    transformerVariantGroup,
} from "unocss";

// https://vitejs.dev/config/
const root = process.cwd();
const nodeEnv = process.env.NODE_ENV || "development";
const env = loadEnv(nodeEnv, root); // 使用空字符串作为前缀
const pathSrc = path.resolve(__dirname, "src");
export default defineConfig({
    base: env.VITE_PUBLIC_PATH,
    root: root,
    resolve: {
        alias: {
            "~/": `${pathSrc}/`,
        },
    },
    plugins: [
        vue(),
        visualizer({
            open: true, // 打包后自动打开可视化网页
        }),
        Unocss({
            presets: [
                presetUno(),
                presetAttributify(),
                presetIcons({
                    scale: 1.2,
                    warn: true,
                }),
            ],
            transformers: [transformerDirectives(), transformerVariantGroup()],
        }),
    ],
    server: {
        port: 4200, // 设置运行端口为4200
    },
    define: {
        "process.env": env,
        // 其他需要替换的环境变量
    },
});
