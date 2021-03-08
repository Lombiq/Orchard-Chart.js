'use strict';

var gulp = require('gulp');
var all = require('gulp-all');

var copyAssets = function (assets, destination) {
    return all(assets.map((asset) => gulp.src(asset.path).pipe(gulp.dest(destination + '/' + asset.name))));
};

module.exports = copyAssets;