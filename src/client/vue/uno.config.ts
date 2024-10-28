import { defineConfig, presetUno, presetAttributify } from 'unocss'
import { inflateSync } from 'zlib'

export default defineConfig({
  presets: [
    presetUno(),
    presetAttributify(),
  ],
  theme: {
    colors: {
      primary: 'var(--el-color-primary)', 
      success: 'var(--el-color-success)', 
      warning: 'var(--el-color-warning)', 
      danger: 'var(--el-color-danger)', 
      error: 'var(--el-color-error)', 
      info: 'var(--el-color-info)', 
    },
  },
})