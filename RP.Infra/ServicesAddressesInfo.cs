using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RP.Infra
{
    public interface IServicesInfo
    {
        IReadOnlyDictionary<string, ServiceInfo> GetAll();
        ServiceInfo? GetService(string serviceName);
    }

    public class ServicesInfo : IServicesInfo
    {
        private readonly Dictionary<string, ServiceInfo> _services;

        public ServicesInfo()
        {
            _services = new Dictionary<string, ServiceInfo>();

            var envVars = Environment.GetEnvironmentVariables();

            foreach (DictionaryEntry envVar in envVars)
            {
                var key = (string)envVar.Key;
                var value = envVar.Value?.ToString();

                if (key.StartsWith("Services:", StringComparison.OrdinalIgnoreCase))
                {
                    var splitKey = key.Split(":");
                    if (splitKey.Length != 3) continue;

                    var serviceUniqueName = splitKey[1];
                    var serviceInfoType = splitKey[2];

                    if (!_services.ContainsKey(serviceUniqueName))
                        _services[serviceUniqueName] = new ServiceInfo();

                    switch (serviceInfoType)
                    {
                        case "Ip":
                            _services[serviceUniqueName].Ip = value ?? string.Empty;
                            break;
                        case "RestApiPort":
                            _services[serviceUniqueName].RestApiPort = value ?? string.Empty;
                            break;
                        case "RestApiSecured":
                            _services[serviceUniqueName].RestApiSecured = string.IsNullOrEmpty(value) ? false : bool.Parse(value);
                            break;
                        case "SupportProberMonitor":
                            _services[serviceUniqueName].SupportPublisherCacheMonitors = string.IsNullOrEmpty(value) ? false : bool.Parse(value);
                            break;
                    }
                }
            }
        }

        public IReadOnlyDictionary<string, ServiceInfo> GetAll() => _services;

        public ServiceInfo? GetService(string serviceName) =>
            _services.TryGetValue(serviceName, out var info) ? info : null;
    }


    /// <summary>
    /// Knows how to collect all services info that injected as enviroment varbiales with the next template
    /// Template: key->"Service:ServiceName:Ip" example value->"127.0.0.1"
    /// Template: key->"Service:ServiceName:RestApiPort" example value->"5043"
    /// Template: key->""Service:ServiceName:SupportProberMonitor" example value->"true"
    /// </summary>
    /*public class ServicesInfo
    {
        public static ServicesInfo Singleton { get; private set; } = new ServicesInfo();

        public Dictionary<string, ServiceInfo> Services = new Dictionary<string, ServiceInfo>();

        private ServicesInfo()
        {
            var envVars = Environment.GetEnvironmentVariables();

            foreach (DictionaryEntry envVar in envVars)
            {
                var key = (string)envVar.Key;
                var value = (string?)envVar.Value;

                if (key.StartsWith("Services:"))
                {
                    var splitKey = key.Split(":");
                    var serviceUniqueName = splitKey[1];
                    var serviceInfoType = splitKey[2];

                    if (!Services.ContainsKey(serviceUniqueName))
                        Services.Add(serviceUniqueName, new ServiceInfo());

                    switch (serviceInfoType)
                    {
                        case "Ip":
                            Services[serviceUniqueName].Ip = string.IsNullOrEmpty(value) ? string.Empty : value;
                            break;
                        case "RestApiPort":
                            Services[serviceUniqueName].RestApiPort = string.IsNullOrEmpty(value) ? string.Empty : value;
                            break;
                        case "SupportProberMonitor":
                            Services[serviceUniqueName].SupportPublisherCacheMonitors = string.IsNullOrEmpty(value) ? false : bool.Parse(value);
                            break;
                    }
                }
            }
        }
    }
    */
    public class ServiceInfo
    {
        public string UniqueName { get; set; }
        public string Ip { get; set; }
        public string RestApiPort { get; set; }
        public bool RestApiSecured { get; set; }
        public bool SupportPublisherCacheMonitors { get; set; }
        
    }
}
