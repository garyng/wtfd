![](./design/banners/Banner-with-bg.png)

Documentation for folders.

[![Build Status](https://dev.azure.com/garyng/wtfd/_apis/build/status/garyng.wtfd?branchName=master)](https://dev.azure.com/garyng/wtfd/_build/latest?definitionId=1&branchName=master)

## Installation

Get this from [chocolatey](https://chocolatey.org/packages/wtfd/):

```
choco install wtfd
```

## Development

This project uses [Cake.Recipe](https://github.com/cake-contrib/Cake.Recipe) for compilation and deployment.

```console
.\build.ps1
```

## Deployment

1. Merge to master for release (from `release` or `develop` branch)
1. Draft a new release on GitHub
   > **Note**: Don't start tag with `v`, otherwise `GitReleaseManager` will fail.
1. `git tag` and push
1. Let ci server run
1. Publish release on GitHub