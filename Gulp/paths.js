const nodeModulesBasePath = './node_modules/';
const distBasePath = './wwwroot/';
const lombiqBasePath = './Assets/Scripts/';
const lombiqDistPath = distBasePath + 'lombiq/';

module.exports = {
    vendorAssets: [
        {
            name: 'chart.js',
            path: nodeModulesBasePath + 'chart.js/dist/**'
        },
    ],
    lombiqAssets: {
        base: lombiqBasePath,
        all: lombiqBasePath + '**/*.js'
    },
    dist: {
        vendors: distBasePath + 'vendors/',
        lombiq: distBasePath + 'lombiq/'
    }
};
