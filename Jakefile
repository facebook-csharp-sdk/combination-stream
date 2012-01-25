var helper = {},
	path = require('path'),
    fs = require('fs'),
	root = path.normalize(__dirname),
    version = fs.readFileSync(path.join(root, 'VERSION'), 'utf-8'),
    nugetVersion = '1.6.0';

task('default', ['dist']);

directory('dist/');
directory('working/');

desc('build')
task('build', function () {
    helper.exec(helper.getDotNetVersionPath('net4.0', 'x86') + 'MSBuild.exe', [
		path.join(root, 'src/CombinationStream-Net20.sln'),
		'/p:Configuration=Release'
	],
	function (code) {
	    code === 0 ? complete() : fail('msbuild failed');
	});

}, { async: true });

desc('clean')
task('clean', function () {
    helper.exec(helper.getDotNetVersionPath('net4.0', 'x86') + 'MSBuild.exe', [
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
    task('pack', ['working/', 'dist/'], function () {
		var file = fs
					.readFileSync(path.join(root, 'src/CombinationStream-Net20/CombinationStream.cs'), 'utf-8')
					.replace('namespace CombinationStream', 'namespace $rootnamespace$');
		fs.writeFileSync(path.join(root, 'working/CombinationStream.cs.pp'), file);
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

desc('create distribution package');
task('dist', ['build', 'nuget:pack']);

/************** HELPER METHODS **************************************************/

(function () {
    var spawn = require('child_process').spawn;

    helper.exec = function (cmd, opts, callback) {
		console.log(cmd);
        var command = spawn(cmd, opts, { customFds: [0, 1, 2] });
        command.on('exit', function (code) { callback(code); });
    };

    helper.getWinDir = function () {
        var winDir = process.env.WINDIR;
        return path.normalize((winDir.substr(-1) === '/' || winDir.substr(-1) === '\\') ? winDir : (winDir + '/')); // append / if absent
    };

	helper.dotNetVersionMapper = {
		'processor': {
			'x86': 'Framework',
			'x64': 'Framework64'
		},
		'version': {
			'net1.0': '1.0.3705',
			'net1.1': '1.1.4322',
			'net2.0': '2.0.50727',
			'net3.5': '3.5',
			'net4.0': '4.0.30319'
		}	
	};

    helper.getDotNetVersionPath = function (version, processor) {

        // http://msdn.microsoft.com/en-us/library/dd414023.aspx
        // http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package#Grouping_Assemblies_by_Framework_Version

        // make it processor instead of bit, just incase MS releases FrameworkARM ;)
        
		var actualProcessor = helper.dotNetVersionMapper['processor'][processor];
		var actualVersion = helper.dotNetVersionMapper['version'][version];
		if(typeof actualProcessor === 'undefined' || typeof actualVersion === 'undefined') {
			fail('specified .NET framework is not supported : ' + version + '(' + processor + ')');
			return;
		}
		
		var netFrameworkPath= helper.getWinDir() + 
									'Microsoft.Net\\' +
									helper.dotNetVersionMapper['processor'][processor] + '\\v' +
									helper.dotNetVersionMapper['version'][version] + '\\';
		return netFrameworkPath;
    };

})();

/************** END OF HELPER METHODS **************************************************/
