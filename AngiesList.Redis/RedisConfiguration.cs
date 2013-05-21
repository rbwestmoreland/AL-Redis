using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Xml;

namespace AngiesList.Redis
{
    public class RedisConfiguration
    {
        public RedisConfiguration()
        {
            Host = "localhost";
            Port = 6379;
        }


        #region Config properties
        /// <summary>
        /// Hostname of redis server. Defaults to localhost.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port number of redis server. Defaults to 6379.
        /// </summary>
        public int Port { get; set; }
        #endregion


        #region Constants

        protected const string CACHE_KEY = "KeyValueStoreConfiguration";
        protected const string SETTINGS_SECTION = "KeyValueStore/Master";
        protected const string CONFIG_FILE = @"KeyValueStore.config";
        #endregion Constants

        #region Read from file

        /// <summary>
        /// Reads the configuration file.
        /// </summary>
        /// <param name="path">The path to the config file. Defaults to KeyValueStore.config</param>
        /// <param name="watchFile">If the config file should be watched and automatically reloaded.</param>
        /// <returns></returns>
        public static RedisConfiguration ReadConfigFile(string path = CONFIG_FILE)
        {
            
            string fullPath;
            if (HttpContext.Current == null)
            {
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            }
            else
            {
                fullPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), path);
            }

            var xmlDoc = LoadXmlDocFromPath(fullPath);
            
            var node = xmlDoc.SelectSingleNode(SETTINGS_SECTION);

            var config = new RedisConfiguration();
            if (node != null && node.Attributes != null)
            {
                if (node.Attributes["host"] != null)
                {
                    config.Host = node.Attributes["host"].Value;
                }

                if (node.Attributes["port"] != null)
                {
                    config.Port = Int32.Parse(node.Attributes["port"].Value);
                }
            }

            return config;
        }

        private static XmlDocument LoadXmlDocFromPath(string path)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            return xmlDoc;
        }

        #endregion

    }

}
