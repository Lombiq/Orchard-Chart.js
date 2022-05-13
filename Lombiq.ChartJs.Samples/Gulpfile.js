const gulp = require('gulp');
const watch = require('gulp-watch');

const scssTargets = require('../../../Utilities/Lombiq.Gulp.Extensions/Tasks/scss-targets');

// It's handy to define all the paths beforehand.
const assetsBasePath = './Assets/';
const distBasePath = './wwwroot/';
const stylesBasePath = assetsBasePath + 'Styles/';
const stylesDistBasePath = distBasePath + 'css/';
const imagesPath = assetsBasePath + 'Images/**/*';
const imagesDistBasePath = distBasePath + 'images/';

gulp.task('build:styles', scssTargets.build(stylesBasePath, stylesDistBasePath));

gulp.task('copy:images', () => gulp
    .src(imagesPath)
    .pipe(gulp.dest(imagesDistBasePath)));

gulp.task('default', gulp.parallel('build:styles', 'copy:images'));

gulp.task('watch', () => {
    watch(stylesBasePath + '**/*.scss', { verbose: true }, gulp.series('build:styles'));
    watch(imagesPath, { verbose: true }, gulp.series('copy:images'));
});
