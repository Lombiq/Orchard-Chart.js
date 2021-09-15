const gulp = require('gulp');
const paths = require('./Gulp/paths');
const copyAssets = require('./Gulp/tasks/copy-assets');

gulp.task('copy:vendor-assets', () => copyAssets(paths.vendorAssets, paths.dist.vendors));
gulp.task('default', gulp.parallel('copy:vendor-assets'));
