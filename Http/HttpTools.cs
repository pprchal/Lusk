using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lusk
{
    internal class HttpTools
    {
        static IEnumerable<string> AsLines(StringReader reader)
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line?.Length == 0)
                {
                    break;
                }
                yield return line;
            }
        }

        internal static IEnumerable<string> ParseHeaders(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line?.Length == 0)
                {
                    break;
                }
                yield return line;
            }
        }

        internal static IEnumerable<string> ParseHeaders2(StringReader reader)
        {
            return AsLines(reader).TakeWhile(line => !string.IsNullOrEmpty(line));
        }

#if DOTNET4
        public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }
#endif        
    }
}
