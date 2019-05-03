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
                files: 'Scripts/*.js',
                tasks: ['concat'],
                options: { atBegin: true }
            }
        },

        concatAdmin:
        {
            dist:
            {
                src:
                    [
                        'Scripts/admin/manage-repos.js'
                    ],
                dest: 'Content/Scripts/admin-panel.js'
            }
        },

        watchAdmin:
        {
            scripts:
            {
                files: 'Scripts/admin/*.js',
                tasks: ['concatAdmin'],
                options: { atBegin: true }
            }
        }

    });

    grunt.loadTasks('Tasks');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.renameTask('concat', 'concatAdmin');
    grunt.loadNpmTasks('grunt-contrib-concat');

    grunt.renameTask('watch', 'watchAdmin');
    grunt.loadNpmTasks('grunt-contrib-watch');
};