# About
**dTerm** is a general purpose management terminal. It's primary objective was to allow multiple console instances to be visible under a single window. It currently supports embedding *Command Prompt*, *Git Bash*, *Power Shell* and *WSL (Ubuntu)* processes.

![Overview](/media/dterm.gif?raw=true "Overview")

## Running

- From code 
  - Get Visual Studio 2017 Community v15.4+
  - `git clone https://github.com/akasarto/d-term.git`
  - Make sure nuget packages are set to auto restore
  - Set _dTerm.UI.Wpf_ as the startup project.
  - F5 and you're good to go.

- From Installer *
  - Go to the [releases](/releases/latest) page and download the *dTermSetup.zip*
  - Extract the setup executable and execute

\* The setup executable is a self signed ClickOnce installer that pulls data from GitHub itself. You may receive alerts from widows SmartScreen regarding unknown publisher and you can also check the intaller binariesf under the *installer* branch.

## Thanks to the owners of the awesome libraries

- [WinApi](https://github.com/prasannavl/WinApi)
- [MaterialDesignInXamlToolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit)


## License

MIT License

Copyright (c) 2017 Thiago Alberto Schneider

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.