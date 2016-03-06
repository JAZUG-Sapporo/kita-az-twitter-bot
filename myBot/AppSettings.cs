using System.Configuration;

namespace myBot
{
    [System.Diagnostics.DebuggerNonUserCodeAttribute]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    public static class AppSettings
    {
        public static string ClientValidationEnabled
        {
            get { return ConfigurationManager.AppSettings["ClientValidationEnabled"]; }
        }

        public static class Demo
        {
            public static string Enabled
            {
                get { return ConfigurationManager.AppSettings["demo:Enabled"]; }
            }
        }

        public static class Errormail
        {
            public static string From
            {
                get { return ConfigurationManager.AppSettings["errormail:from"]; }
            }

            public static string Subject
            {
                get { return ConfigurationManager.AppSettings["errormail:subject"]; }
            }

            public static string To
            {
                get { return ConfigurationManager.AppSettings["errormail:to"]; }
            }
        }

        public static class Key
        {
            public static string Twitter
            {
                get { return ConfigurationManager.AppSettings["Key.Twitter"]; }
            }
        }

        public static string Salt
        {
            get { return ConfigurationManager.AppSettings["Salt"]; }
        }

        public static class Site
        {
            public static string Timezone
            {
                get { return ConfigurationManager.AppSettings["site:timezone"]; }
            }
        }

        public static class Smtp
        {
            public static string Config
            {
                get { return ConfigurationManager.AppSettings["smtp:config"]; }
            }
        }

        public static string UnobtrusiveJavaScriptEnabled
        {
            get { return ConfigurationManager.AppSettings["UnobtrusiveJavaScriptEnabled"]; }
        }

        public static class Webpages
        {
            public static string Enabled
            {
                get { return ConfigurationManager.AppSettings["webpages:Enabled"]; }
            }

            public static string Version
            {
                get { return ConfigurationManager.AppSettings["webpages:Version"]; }
            }
        }
    }
}

