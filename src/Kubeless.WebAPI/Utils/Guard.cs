namespace Kubeless.WebAPI.Utils
{
    using System;
    
    public static class Guard 
    {
        public static void AgainstEmpty(string value, string parameterName) 
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
