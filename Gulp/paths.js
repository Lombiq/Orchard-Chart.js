const nodeModulesBasePath = './node_modules/';
const distBasePath = './wwwroot/';
const lombiqBasePath = './Assets/Scripts/';

module.exports = {
    vendorAssets: [
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
    ],
    lombiqAssets: {
        base: lombiqBasePath,
        all: lombiqBasePath + '**/*.js',
    },
    dist: {
        vendors: distBasePath + 'vendors/',
        lombiq: distBasePath + 'lombiq/',
    },
};
