![](./design/banners/Banner-with-bg.png)

Documentation for folders.

[![Build Status][azure-pipelines-badge]][azure-pipelines]

| | Release | Pre-release
---------|----------|---------
GitHub | [![GitHub][gh-badge]][gh] | [![GitHub][gh-badge-pre]][gh]
Chocolatey | [![Chocolatey][choco-badge]][choco] | [![Chocolatey][choco-badge-pre]][choco]

## Quick Links

- [ ] todo

## Installation

Get this from [Chocolatey][choco]:

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

[azure-pipelines]: https://dev.azure.com/garyng/wtfd/_build/latest?definitionId=1&branchName=master
[azure-pipelines-badge]: https://dev.azure.com/garyng/wtfd/_apis/build/status/garyng.wtfd?branchName=master
[choco]: https://chocolatey.org/packages/wtfd/
[choco-badge]: https://img.shields.io/chocolatey/v/wtfd
[choco-badge-pre]: https://img.shields.io/chocolatey/v/wtfd
[gh]: https://github.com/garyng/wtfd/releases
[gh-badge]: https://img.shields.io/github/v/release/garyng/wtfd
[gh-badge-pre]: https://img.shields.io/github/v/release/garyng/wtfd?include_prereleases