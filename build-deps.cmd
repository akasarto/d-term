@echo off

rem Setup windows terminal env variables.
call ./modules/ms-terminal/tools/razzle.cmd

rem Navigate to the dependency project that we depend on.
cd modules/ms-terminal/src/cascadia/PublicTerminalCore

rem build the PublicTerminalCore.dll from win terminal
call bcx rel
