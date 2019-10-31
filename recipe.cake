// #load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease
#load "./cake/Cake.Recipe/Content/*.cake"

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "wtfd",
                            repositoryOwner: "garyng",
                            repositoryName: "wtfd",
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false,
                            shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();

// // todo: remove Cake.Issues.Recipe
// // todo: remove dupfinder, inspectcode

// // todo: update test report (check gitversion)