module.exports = function (grunt)
{
    grunt.initConfig
    ({
        concat:
        {
            dist:
            {
                files:
                {
                    'Content/Scripts/main.js': ['Scripts/**.js']
                }
            }
        }
    });
};