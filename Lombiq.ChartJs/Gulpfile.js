const recommendedSetup = require('../../../Utilities/Lombiq.Gulp.Extensions/recommended-setup');

const nodeModulesBasePath = './node_modules/';

const assets = [
    {
        name: 'chart.js',
        path: nodeModulesBasePath + 'chart.js/dist/**',
    },
    {
        name: 'chartjs-plugin-annotation',
        path: nodeModulesBasePath + 'chartjs-plugin-annotation/chart*.js',
    },
    {
        name: 'chartjs-plugin-datalabels',
        path: nodeModulesBasePath + 'chartjs-plugin-datalabels/dist/**',
    },
];

recommendedSetup.setupVendorsCopyAssets(assets);
