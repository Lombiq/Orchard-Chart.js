const gulp = require('gulp');
const paths = require('./Gulp/paths');
const jsTargets = require('../../Utilities/Lombiq.Gulp.Extensions/Tasks/js-targets');
const copyAssets = require('./Gulp/tasks/copy-assets');

gulp.task('copy:vendor-assets', () => copyAssets(paths.vendorAssets, paths.dist.vendors));
gulp.task('build:lombiq-js', () => jsTargets.compile(paths.lombiqAssets.base, paths.dist.lombiq));
gulp.task('default', gulp.parallel('copy:vendor-assets', 'build:lombiq-js'));
