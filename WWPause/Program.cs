/*
    ============================================================================

    Namespace:			WWPause

    Class Name:			Program

	File Name:			Program.cs

    Synopsis:		    The objective of this module is dead simple; block a
                        script until one of two keys, RETURN (ENTER) and CTRL-C.

    Remarks:			This assembly supersedes a Visual C program that I wrote
                        many years ago.

                        In addition to 64-bit goodness, this program dispenses
                        with the asynchronous event that interfered with getting
                        a clipboard copy of program output.

    Author:				David A. Gray

    License:            Copyright (C) 2024, David A. Gray.
                        All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ --------------------------------------------------
	2024/11/28 4.0.4   DAG    Initial implementation created, tested, and
                              deployed as an upgrade of the Visual C program
                              that it supersedes. For the purpose of version
                              numbering, this program is treated as a major 
                              upgrade of it native  code predecessor.

    2024/12/01 4.0.11  DAG    Protect against the InvalidOperationException
                              Exception documented as arising when ReadKey is
                              invoked and the standard stream is redirected, and
                              set the Console.TreatControlCAsInput property to
                              TRUE to coerce Windows to allow this program to
                              intercept the Ctrl-C key pair.
    ============================================================================
*/


using System;

using System.IO;
using System.Reflection;

namespace WWPause
{
	internal class Program
	{
        static void Main ( string [ ] args )
        {
            const int ARRAY_FIRST_ELEMENT = 0;

            const bool SUPPRESS_KEY_ECHO = true;

            const string TIME_TEMPLATE = @"{0} {1}";
            const string TIME_FORMAT = @"ddd dd MMM yyyy HH:mm:ss";

			//  ----------------------------------------------------------------
            //  Since the first catch block evaluates its value to determine the
            //  correct message to display, this flag needs method scope.
            //
            //  The main routine sets its value to TRUE before it executes the
            //  ReadKey method that raises an InvalidOperationException
            //  Exception when the console stream is redirected, and immediately
            //  clears it when control falls through, as it otherwise will.
			//  ----------------------------------------------------------------

			bool fReading = false;

            try
            {
                DateTime dtmNow = DateTime.UtcNow;
                Assembly me = Assembly.GetEntryAssembly ( );

                AssemblyInformationalVersionAttribute [ ] attr = me.GetCustomAttributes ( typeof ( AssemblyInformationalVersionAttribute ) , false ) as AssemblyInformationalVersionAttribute [ ];

                Console.WriteLine (
                    Properties.Resources.IDS_VERSION_TEMPLATE ,                     // Format Control String: {0} version {1}, {2}, {3}
                    Path.GetFileNameWithoutExtension ( me.Location ) ,              // Format Item 0: {0} version
                    attr [ ARRAY_FIRST_ELEMENT ].InformationalVersion );            // Format Item 1: version {1},
                Console.WriteLine (
                    TIME_TEMPLATE ,
                    dtmNow.ToString ( TIME_FORMAT ) ,
                    Properties.Resources.IDS_NOW_UTC );
                Console.WriteLine (
                    TIME_TEMPLATE ,
                    dtmNow.ToUniversalTime ( ).ToString ( TIME_FORMAT ) ,
                    Properties.Resources.IDS_NOW_LOCAL );
                Console.Write ( Properties.Resources.IDS_PROMPT );

				//  ------------------------------------------------------------
				//  Though the documentation recommends caution in using this
				//  method to allow a console program to prcess Ctrl-C, I chose
				//  to disregard that caution so that I can guarantee that the
				//  console scrolls up as intended.
				//
				//  Reference: ConsoleKeyInfo.Key Property
                //             https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo.key?view=netframework-4.8#system-consolekeyinfo-key
				//  ------------------------------------------------------------

				Console.TreatControlCAsInput = true;

				bool fWaiting = true;

                while ( fWaiting )
                {
                    fReading = true;
					ConsoleKeyInfo cki = Console.ReadKey ( SUPPRESS_KEY_ECHO );
                    fReading = false;

                    switch ( cki.Key )
                    {
                        case ConsoleKey.C:
                            if ( cki.Modifiers == ConsoleModifiers.Control )
                            {
                                fWaiting = false;
                            }   // Reject the C character unless the Control key accompanies it.
                            break;
                        case ConsoleKey.Enter:
                            fWaiting = false;
                            break;
					}   // switch ( cki.Key )
				}   // while ( fWaiting )
            }
            catch ( InvalidOperationException exInvOp )
            {
                if ( fReading )
                {
                    Console.Write (
                        Properties.Resources.IDS_ERRMSG_REDIRECTD_INPUT_STREAM ,// Format Control String: The input stream is redirected away from the console.{0}Since this configuration is unsupported and meaningless,{0}script execution is resuming now.
						Environment.NewLine );                                  // Format Item 0: Substitute a newline at console.{0}Since ABD meaningless,{0}script,
				}   // TRUE (An InvalidOperationException Exception is anticipated if the input stream is redirected.) block, if ( fReading )
				else
                {
					Console.WriteLine (
						Properties.Resources.IDS_ERRMSG_GENERIC_EXCEPTION ,     // Format Control String: An {0} Exception arose: {1}{2}Program terminating now.
						exInvOp.GetType ( ).FullName ,                          // Format Item 0: An {0} Exception
						exInvOp.Message ,                                       // Format Item 1: arose: {1}
						Environment.NewLine );                                  // Format Item 2: {2}Program terminating now.
				}   // FALSE (Though the InvalidOperationException Exception is unanticipated, it must be handled, neverthless.) block, if ( fReading )
			}
            catch ( Exception ex )
            {
                Console.WriteLine (
                    Properties.Resources.IDS_ERRMSG_GENERIC_EXCEPTION ,         // Format Control String: An {0} Exception arose: {1}{2}Program terminating now.
                    ex.GetType ( ).FullName ,                                   // Format Item 0: An {0} Exception
                    ex.Message ,                                                // Format Item 1: arose: {1}
                    Environment.NewLine );                                      // Format Item 2: {2}Program terminating now.
            }
            finally
            {
                Console.WriteLine ( );
            }
		}  // static void Main
	}   // internal class Program
}   // namespace WWPause