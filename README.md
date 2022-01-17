# About
**dTerm** is a general purpose tool for developers and it's primary intent is to help organize multiple console applications under a single, centralized manager.

![Overview](/media/dTerm.png?raw=true "Overview")

## Running From Code

There are a few pre-requirements that you must have in mind before running it locally:

- Windows 10 1903 (build 18362) or later.
- Visual Studio 2019 Community edition or higher.
- Ef core tools for migrations (`dotnet tool install --global dotnet-ef`).

After that, just follow the steps bellow and everything should work as expected:

- `git clone https://github.com/akasarto/d-term.git`.
- `cd` into the directory where the repository was clonned (`cd d-term`).
- Update submodues with `git submodule update --init --recursive --depth 1`.
- Build the windows terminal dependencies with `./build-deps.cmd`.
- Open `dTerm.sln` in visual studio and restore the nuget packages.
- Set `dTerm.UI.Wpf` as the startup project.
- Do a full solution build.
- Hit F5 to start.

## Upcoming changes and features

- Allow integration with MQTT servers.
- Add named pipes IPC server infrastructure.

## Known Bugs

- App main window may lose focus when exiting/closing consoles.

## Thanks

- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)


