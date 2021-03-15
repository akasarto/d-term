# About
**dTerm** is a general purpose tool for developers and it's primary intent is to help organize multiple console applications under a single, centralized manager.

![Overview](/media/dTerm.png?raw=true "Overview")

## Running

- From Portable Package
  - Make sure your system has .Net Framework 4.7 or higher installed.
  - Go to the [releases](https://github.com/akasarto/d-term/releases/latest) page and download the corresponding portable _.zip_ file.
  - Extract the files to a location of your choosing and execute _dTerm.exe_.

- From code 
  - Get Visual Studio 2019 Community or higher.
  - `git clone https://github.com/akasarto/d-term.git`.
  - `cd` into the directory you just clonned the repository (`cd d-term`).
  - Update submodues with `git submodule update --init --recursive --depth 1`.
  - Buld the windows terminal dependencies with `./build-deps.cmd`.
  - Open visual studio and restore the nuget packages.
  - Set `dTerm.UI.Wpf` as the startup project.
  - Do a full solution build.
  - Hit F5 to start.

## Upcoming changes and features

- Allow integration with MQTT servers.
- Add named pipes IPC server infrastructure.

## Known Bugs

- App main window may lose focus when exiting/closing consoles.

## Thanks

- [LiteDB](http://www.litedb.org/)
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [ReactiveUI](https://reactiveui.net/)


