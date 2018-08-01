using Microsoft.Win32;
using System.Linq;
using System.Reflection;

namespace AudioSwitcher.Services
{
    public class StartupService
    {
        private const string ValueName = "AudioSwitchers";

        public bool AdicionarApp()
        {
            try
            {
                using (var registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (registry.GetValueNames().Contains(ValueName) && !this.RemoverApp())
                        return false;

                    registry.SetValue(ValueName, Assembly.GetEntryAssembly().Location);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool RemoverApp()
        {
            try
            {
                using (var registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    registry.DeleteValue(ValueName, false);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}