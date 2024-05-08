const path = require('path');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
module.exports = {
  entry: './scripts/index.ts', // 入口文件
  output: {
    path: path.resolve(__dirname, 'wwwroot/js'), // 输出目录
    filename: 'tailwind.js' // 输出文件名
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/, // 匹配.ts或.tsx文件
        use: 'ts-loader',
        exclude: /node_modules/
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.jsx', '.ts', '.tsx'] // 添加这一行
  },
  //plugins: [
  //  new BundleAnalyzerPlugin()
  //],
  stats: {
    colors: true,
    modules: false,
    children: false,
    chunks: false,
    chunkModules: false,
    warnings: true,
    errors: true
  }
};
