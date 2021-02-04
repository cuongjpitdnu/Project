using System.Collections.Generic;

namespace GPMain.Common.Navigation
{
    public class NavigationParameters : Dictionary<string, object>
    {
        private const string KEY_DEFAULT = "Default";

        public bool TryGetValue<T>(string key, out T value)
        {
            if (base.TryGetValue(key, out object result))
            {
                if (result is T variable)
                {
                    value = variable;
                    return true;
                }
            }

            value = default(T);

            return false;
        }

        public bool TryGetValue<T>(out T value)
        {
            if (TryGetValue<T>(KEY_DEFAULT, out value))
            {
                return true;
            }

            return false;
        }

        public T GetValue<T>(string keyGet = KEY_DEFAULT, T defaultValue = default(T))
        {
            T rstValue = defaultValue;
            TryGetValue<T>(keyGet, out rstValue);
            return rstValue;
        }

        public NavigationParameters()
        {
        }

        public NavigationParameters(object defaultParameter)
        {
            Add(KEY_DEFAULT, defaultParameter);
        }
    }
}
