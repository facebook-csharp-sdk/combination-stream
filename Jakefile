var helper = {},
	path = require('path'),
    fs = require('fs'),
	root = path.normalize(__dirname),
    version = fs.readFileSync(path.join(root, 'VERSION'), 'utf-8'),
    nugetVersion = '1.6.0';

directory('dist');

desc('build')
task('build', function () {
    helper.exec(helper.getDotNetVersionPath('net40', 'x86') + 'MSBuild.exe', [
		path.join(root, 'src/CombinationStream-Net20.sln'),
		'/p:Configuration=Release'
	],
	function (code) {
	    code === 0 ? complete() : fail('msbuild failed');
	});

}, { async: true });

desc('clean')
task('clean', function () {
    helper.exec(helper.getDotNetVersionPath('net35', 'x86') + 'MSBuild.exe', [
		path.join(root, 'src/CombinationStream-Net20.sln'),
		'/p:Configuration=Release',
		'/target:Clean'
	],
	function (code) {
	    code === 0 ? complete() : fail('msbuild failed');
	});

}, { async: true });

namespace('nuget', function () {

    desc('create nuget package');
    task('pack', ['dist'], function () {
        helper.exec(path.join(root, 'src/packages/NuGet.CommandLine.' + nugetVersion, 'tools/NuGet.exe'), [
            'pack',
            path.join(root, 'src/CombinationStream.nuspec'),
            '-Version',
            version,
            '-OutputDirectory',
            path.join(root, 'dist')
        ],
        function (code) {
            code === 0 ? complete() : fail('nuget pack failed');
        });
    }, { async: true });
});

task('default', ['build', 'nuget:pack']);

/************** HELPER METHODS **************************************************/

(function () {
    var spawn = require('child_process').spawn;

    helper.exec = function (cmd, opts, callback) {
        var command = spawn(cmd, opts, { customFds: [0, 1, 2] });
        command.on('exit', function (code) { callback(code); });
    };

    helper.getWinDir = function () {
        var winDir = process.env.WINDIR;
        return winDir.substr(-1) === '/' ? winDir : (winDir + '/'); // append / if absent
    };

    helper.getDotNetVersionPath = function (version, processor) {

        // http://msdn.microsoft.com/en-us/library/dd414023.aspx
        // http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package#Grouping_Assemblies_by_Framework_Version

        // make it processor instead of bit, just incase MS releases FrameworkARM ;)
        // TODO: CACHE

        var netFramework = helper.getWinDir() + "Microsoft.Net/" + (processor == 'x86' ? 'Framework' : 'Framework64') + '/';
        switch (version) {
            case 'net10':
                return netFramework + 'v1.0.3705/';
            case 'net11':
                return netFramework + 'v1.1.4322/';
            case 'net20':
                return netFramework + 'v2.0.50727/';
            case 'net35':
                return netFramework + 'v3.5/';
            case 'net40':
                return netFramework + 'v4.0.30319/';
            default:
                fail('specified .NET framework is not supported : ' + version + '(' + processor + ')');
                break;
        }
    };

})();

/************** END OF HELPER METHODS **************************************************/
