namespace GhostScriptPCL.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    namespace GhostscriptPCL.API
    {
        internal class GhostScriptPCL64
        {
            #region Hooks into Ghostscript PDL's 64-bit pcl6-64.dll
            
            [DllImport("pcl6-64.dll", EntryPoint = "pl_main")]
            private static extern int InitAPI(int argc, string[] argv);

            #endregion

            /// <summary>
            /// GS can only support a single instance, so we need to bottleneck any multi-threaded systems.
            /// </summary>
            private static object resourceLock = new object();

            /// <summary>
            /// Calls the Ghostscript API with a collection of arguments to be passed to it
            /// </summary>
            public static void CallAPI(string[] args)
            {
                lock (resourceLock)
                {
                    try
                    {
                        int result = InitAPI(args.Length, args);

                        if (result < 0)
                        {
                            throw new ExternalException("Ghostscript conversion error", result);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExternalException(string.Concat("GhostScriptPCL error ", ex.Message), ex);
                    }
                }
            }
        }
    }
}
