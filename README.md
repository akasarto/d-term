# About
**dTerm** is a general purpose tool for developers. It's primary intent is to allow multiple console instances to be visible under a single window, but it is rapidly turning into something bigger.

![Overview](/media/dTerm.png?raw=true "Overview")

## Running

- From code 
  - Get Visual Studio 2017 Community v15.5+.
  - `git clone https://github.com/akasarto/d-term.git`.
  - Make sure nuget packages are set to auto restore.
  - Set _UI.Wpf_ as the startup project.
  - Compile and you're good to go.
  - Hit F5 to start.

- From Installer
  - Make sure your system has .Net Framework 4.7 or higher installed.
  - Go to the [releases](https://github.com/akasarto/d-term/releases/latest) page and download the corresponding portable _.zip_ file.
  - Extract the files to a location of your choosing and execute _dTerm.exe_.

## Known Bugs

- App main window may lose focus when exiting/closing consoles.

## Upcoming changes and features

- Allow consoles to pipe data to the app (IPC).
- Data export/import features.

## Credits & Thanks

- [LiteDB](http://www.litedb.org/)
- [WinApi](https://github.com/prasannavl/WinApi)
- [MaterialDesignInXamlToolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit)
- [ReactiveUI](https://reactiveui.net/)
- [Dragablz](https://github.com/ButchersBoy/Dragablz)

## Quick Overview

![Consoles](/media/dterm1.gif?raw=true "Transparency")  

![Arranges](/media/dterm2.gif?raw=true "Minimize / Restore")  

## License

MIT License

Copyright (c) 2018 Thiago Alberto Schneider

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

## Donation

If you liked this work, please consider a small donation to help keeping it up.  

[![PayPal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UUA94B6TXGH3L)

