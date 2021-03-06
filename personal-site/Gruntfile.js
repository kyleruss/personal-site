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
                        'Scripts/conf.js',
                        'Scripts/core.js',
                        'Scripts/home.js',
                        'Scripts/about.js',
                        'Scripts/skills.js',
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
                files: 'Scripts/*.js',
                tasks: ['concat'],
                options: { atBegin: true }
            }
        }
    });

    grunt.loadTasks('Tasks');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-watch');
};