module.exports = function (grunt)
{
    grunt.initConfig
    ({
        concat:
        {
            dist:
            {
                src:
                    [
                        'Scripts/core.js',
                        'Scripts/about.js',
                        'Scripts/blog.js',
                        'Scripts/portfolio.js',
                        'Scripts/contact.js'
                    ],
                dest: 'Content/Scripts/main.js'
            }
        },

        watch:
        {
            scripts:
            {
                files: 'Scripts/**/*.js',
                tasks: ['concat'],
                options: { atBegin: true }
            }
        }

    });

    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-watch');
};