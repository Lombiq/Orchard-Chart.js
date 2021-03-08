const gulp = require('gulp');
const all = require('gulp-all');

const copyAssets = function (assets, destination) {
    return all(assets.map((asset) => gulp.src(asset.path).pipe(gulp.dest(destination + '/' + asset.name))));
};

module.exports = copyAssets;
