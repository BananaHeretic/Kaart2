using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaart2
{
    //https://msdn.microsoft.com/en-us/library/windows/apps/ff769510(v=vs.105).aspx
    public class Settings
    {
        Windows.Storage.ApplicationDataContainer localSettings;
        const string CheckBoxSettingKeyName = "CheckBoxSetting";
        const string ListBoxSettingKeyName = "ListBoxSetting";
        const string UsernameSettingKeyName = "UsernameSetting";
        const string PasswordSettingKeyName = "PassowrdSetting";

        const bool CheckBoxSettingDefault = true;
        const int ListBoxSettingDefault = 0;
        const string UsernameSettingDefault = "";
        const string PasswordSettingDefault = "";

        public Settings()
        {
            Main();
        }
        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            if (localSettings.Values.ContainsKey(key))
            {
                if(localSettings.Values[key] != value)
                {
                    localSettings.Values[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                localSettings.Values[key] = value;
                valueChanged = true;
            }
            return valueChanged;
        }
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            if(localSettings.Values.ContainsKey(key))
            {
                value = (T)localSettings.Values[key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }
        public bool CheckBoxSetting
        {
            get
            {
                return GetValueOrDefault<bool>(CheckBoxSettingKeyName, CheckBoxSettingDefault);
            }
            set
            {
                AddOrUpdateValue(CheckBoxSettingKeyName, value);
            }
        }
       public int ListBoxSetting
        {
            get
            {
                return GetValueOrDefault<int>(ListBoxSettingKeyName, ListBoxSettingDefault);
            }
            set
            {
                AddOrUpdateValue(ListBoxSettingKeyName, value);
            }
        }
        public string UsernameSetting
        {
            get
            {
                return GetValueOrDefault<string>(UsernameSettingKeyName, UsernameSettingDefault);
            }
            set
            {
                AddOrUpdateValue(UsernameSettingKeyName, value);
            }
        }
        public string PasswordSetting
        {
            get
            {
                return GetValueOrDefault<string>(PasswordSettingKeyName, PasswordSettingDefault);
            }
            set
            {
                AddOrUpdateValue(PasswordSettingKeyName, value);
            }
        }

        public void Main()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["exampleSetting"] = "Hello";

            Object value = localSettings.Values["exampleSetting"];

            if(value == null)
            {
                System.Diagnostics.Debug.WriteLine("Something went wrong");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(value as string);
            }
            localSettings.Values.Remove("exampleSetting");
        }
    }
}
