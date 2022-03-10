const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        "ccm-form": "./src/ccm-form.ts",
        "ccm-main": "./src/ccm-main.ts",
        "ccm-statistics": "./src/ccm-statistics.ts",
        "ccm-styles": "./styles/less/Site.less"
    },
    output: {
        path: path.resolve(__dirname + "/../CCM.Web/wwwroot/dist"),
        filename: "[name].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    optimization: {
        minimize: false
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            },
            {
                test: /\.less$/i,
                use: [{
                        loader: MiniCssExtractPlugin.loader,
                        options: {
                            publicPath: ''
                        }
                    },
                    "css-loader",
                    "less-loader"
                ]
            },
            {
                test: /\.(eot|ttf|woff|woff2|otf)$/i,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: './static',
                        context: path.resolve(__dirname, './')
                    }
                }]
            },
            {
                test: /\.(jpe?g|png|gif|svg)$/i,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: './static',
                        context: path.resolve(__dirname, './')
                    }
                }]
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin({
            protectWebpackAssets: false,
            cleanStaleWebpackAssets: false,
        }),
        new MiniCssExtractPlugin({
            filename: "[name].css",
            chunkFilename: "[id].css"
        }),
    ]
};