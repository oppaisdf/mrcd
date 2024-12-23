export const plugins = [
    require('postcss-import'),
    require('autoprefixer'),
    require('@fullhuman/postcss-purgecss')({
        content: [
            './dist/sys/browser/**/*.html',
            './dist/sys/browser/**/*.js'
        ],
        defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
    }),
    require('cssnano')({ preset: 'default' })
];
