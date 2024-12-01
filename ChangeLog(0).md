# WWPause Change Log

This file is a running history of fixes and improvements from version 7.0
onwards. Changes are documented for the newest version first. Within each
version, classes are listed alphabetically.

# AssemblyVersion 4.0.0 Updated 2024/12/01, AssemblyFileVersion 4.0.11

Protect against the InvalidOperationException Exception documented as
arising when ReadKey is invoked and the standard stream is redirected, and
set the Console.TreatControlCAsInput property to TRUE to coerce Windows to
allow this program to intercept the Ctrl-C key pair.

This is also the first public release.

# AssemblyVersion 4.0.0 Updated 2024/11/28, AssemblyFileVersion 4.0.4

Initial implementation created, tested, and deployed as an upgrade of the
Visual C program that it supersedes. For the purpose of version numbering,
this program is treated as a major upgrade of it native code predecessor.
