# Package Manager Modules

<p align="center">
	<img alt="GitHub package.json version" src ="https://img.shields.io/github/package-json/v/Thundernerd/Unity3D-PackageManagerModules" />
	<a href="https://github.com/Thundernerd/Unity3D-PackageManagerModules/issues">
		<img alt="GitHub issues" src ="https://img.shields.io/github/issues/Thundernerd/Unity3D-PackageManagerModules" />
	</a>
	<a href="https://github.com/Thundernerd/Unity3D-PackageManagerModules/pulls">
		<img alt="GitHub pull requests" src ="https://img.shields.io/github/issues-pr/Thundernerd/Unity3D-PackageManagerModules" />
	</a>
	<a href="https://github.com/Thundernerd/Unity3D-PackageManagerModules/blob/master/LICENSE.md">
		<img alt="GitHub license" src ="https://img.shields.io/github/license/Thundernerd/Unity3D-PackageManagerModules" />
	</a>
	<img alt="GitHub last commit" src ="https://img.shields.io/github/last-commit/Thundernerd/Unity3D-PackageManagerModules" />
</p>

A toolkit to create modules for the Package Manager Window.

## Installation
1. The package is available on the [openupm registry](https://openupm.com). You can install it via [openupm-cli](https://github.com/openupm/openupm-cli).
```
openupm add net.tnrd.packagemanagermodules
```
2. Installing through a [Unity Package](http://package-installer.glitch.me/v1/installer/package.openupm.com/net.tnrd.packagemanagermodules?registry=https://package.openupm.com) created by the [Package Installer Creator](https://package-installer.glitch.me) from [Needle](https://needle.tools)

[<img src="https://img.shields.io/badge/-Download-success?style=for-the-badge"/>](http://package-installer.glitch.me/v1/installer/package.openupm.com/net.tnrd.packagemanagermodules?registry=https://package.openupm.com)

## Usage

### Creating a module

To create a module you need create a class that implements the `IPackageManagerModule` interface.

These classes get automatically picked up and added to the module menu which can be located at the top of the Package Manager Window

![Modules menu underlined](Documentation~/screenshot_01.png)


### Identifier
The identifier of your module should be unique and only be used for one package. I recommend using the [reverse domain name notation](en.wikipedia.org/wiki/Reverse_domain_name_notation) for this.

### DisplayName
The display name of your module is what will be shown in the modules menu. If you plan on creating multiple modules then you can group them together by using a / (forward slash)

e.g. `Foo/Bar`, `Foo/Baz`, `Foo/Qux`

These will all be grouped together under `Foo`

### IsEnabled
This is to identify if your module is enabled. You have to keep track of this yourself in your module. This will be reflected in the modules menu.

### Initialize()
This is called when the modules are created and should be used for initialization logic only.

### Dispose()
This is called when the Package Manager Window closes or when the editor starts recompiling. Use this to clean up things like event subscriptions.

### Enable()
This is called when the user enables your module through the modules menu.

### Disable()
This is called when the user disables your module through the modules menu. This will also be called when the Package Manager Window closes, or when the editor starts recompiling.

## Sample
This package comes with a Dependencies Editor sample. This sample allows easy editing of the dependencies of a package that is in development. You can import the sample through the Package Manager Window.

![Dependencies editor sample](Documentation~/screenshot_02.png)

## Support
**Package Manager Modules** is a small and open-source utility that I hope helps other people. It is by no means necessary but if you feel generous you can support me by donating.

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/J3J11GEYY)

## Contributing
Pull requests are welcomed. Please feel free to fix any issues you find, or add new features.
