# WWPause ReadMe

`WWPause.exe` is a character-mode (command line) program for use as a robust
replacment for the anemic `pause` command that has been part of the MS-DOS
command parser and its Microsoft Windows successors, `COMMAND.COM` and `CMD.EXE`
since their first release.

From the beinning, and to this day, the problem with `pause` is that it responds
to absolutely __any__ key by allowing the script that invoked it to resume. The
result was countless scripts the resumed before I could read their output to the
point where the `pause` command appeared. Almost invariably, such output is lost
forever, no matter how important it is to you.

Enter `WWPause.exe`, the `pause` command that exerts enough control to give its
users a much greater degree of control over when the invoking script resumes and
its output scrolls off and is forever lost.

To that end, `WWPause.exe` keeps the script halted until one of two keystrokes
appear in the input stream, `CTRL-C` and `ENTER`. All other keyboard input is
silently swallowed, and the script remains paused at the point where the command
appeared in the script sequence.

## Usage

Using the program in your scripts is simple.

1	Place the program in a directory that is on your Windows `PATH` list.

2	Replace every instance of the internal `pause` command with either `WWPause.exe` or plain `WWPause`.

## History

Although this is the first public release of `WWPause.exe`, it is its __fourth__
major version. The first version was implemented in Intel x86 Assembly language,
and dates from 1990. That version worked well until the 64 bit versions of
Microsoft Windows came along and completely eliminated the 16-bit execution
environment. The next version was implemented in C, and built with the Microsoft
Visual C 6.0 compiler and tools. Since this version was implemnted as a 32-bit
program, it ran without issue on all modern Windows computers, since all of them
support both 32 and 64 bit code. A subsequent upgrade, still implented in C,
came along about 4 years ago, and it was built with the current version of the
Microsoft Visual C compiler and tools.

That version would remain relevant today, except for one flaw. It used an
asyncronous event loop to intercept Ctrl-C, and that event loop made capturing
screen text much harder.

This version came to be to eliminate the asynchronous event loop. This version
is implemented in Visual C#, and it was built with Microsoft Visual Studio 2022.
Since its needs are simple, it requires only a working installation of the
Microsoft .NET Framework, version 4.8 or later.

## Road Map

Though it could probably be achieved without much effort, at present, there are
__no plans__ for a .NET Core version. Nevertheless, since it is being released
under the MIT license, anybody who wants to do so is free to fork the repository
and build one. However, unless you are allergic to .NET Framework 4.8 or want to
run it on another platform, such as a Linux distribution, I see no need for it.