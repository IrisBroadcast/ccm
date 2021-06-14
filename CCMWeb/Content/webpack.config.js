const path = require('path')

module.exports = {
    entry: {
        main: './index.js',
       // app: './app.js',
        vendor: ['jquery']
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot'),
    },
    optimization: {
        minimize: false,
    },
    module: {
        rules: [
            {
                test: /\.less$/,
                use: [
                    { 
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader'
                    },
                    {
                        loader: 'less-loader'
                    },
                ],
            },
            {
                test: /\.(png|jpe?g|gif|eot|svg|woff|woff2|ttf)$/i,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                        },
                    },
                ],
            },
        ],
    },
};