pipeline {
	agent {
		label 'unity'
	}
	options {
		disableConcurrentBuilds()
		disableResume()
	}
	stages {
		stage('Index workspace') {
			steps {
				script {
					stash name: 'data', allowEmpty: false, includes: 'Data/**'

					unityProject (
							// define unity project location relative to repository
							LOCATION : '',

							// If given, automatically use these credentials to license a free Unity version.
							UNITY_CREDENTIALS : 'Slothsoft-Unity',
							EMAIL_CREDENTIALS : 'Slothsoft-Google',
							STEAM_CREDENTIALS : 'Slothsoft-Steam',

							// use auto-versioning based on tags+commits
							AUTOVERSION : '',

							// automatically create C# docs
							BUILD_DOCUMENTATION : '0',

							// automatically run Unity Test Runner
							TEST_UNITY : '0',

							// automatically run dotnet format
							TEST_FORMATTING : '0',
							FORMATTING_EXCLUDE : 'Library Assets/Plugins Packages',

							// which executables to create
							BUILD_FOR_WINDOWS : '1',
							BUILD_FOR_LINUX : '0',
							BUILD_FOR_MAC : '0',
							BUILD_FOR_WEBGL : '1',
							BUILD_FOR_ANDROID : '0',

							BUILD_WINDOWS_CALL : { project, build, report ->
								callUnity "unity-build '${project}' '${build}' windows", report
								dir(build) {
									unstash 'data'
								}
							},

							// which platforms to deploy to
							DEPLOY_TO_STEAM : '0',
							DEPLOY_TO_ITCH : '0',
							DEPLOY_ON_FAILURE : '1',
							DEPLOYMENT_BRANCHES : ["/main"],

							// configration for deploying to steam
							STEAM_ID : '',
							STEAM_DEPOT_WINDOWS : '',
							STEAM_DEPOT_LINUX : '',
							STEAM_DEPOT_MAC : '',
							STEAM_BRANCH : '',

							// configuration for deploying to itch
							ITCH_ID : '',
							ITCH_CREDENTIALS : '',

							// configuration for deploying to Discord
							REPORT_TO_DISCORD : '0',
							DISCORD_WEBHOOK : '',
							)
				}
			}
		}
	}
}