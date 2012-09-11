﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REST0.APIService.Descriptors
{
    class Method
    {
        public Service Service { get; set; }
        public string Name { get; set; }
        public string DeprecatedMessage { get; set; }
        public IDictionary<string, ParameterType> ParameterTypes { get; set; }
        public IDictionary<string, Parameter> Parameters { get; set; }
        public string ConnectionString { get; set; }
        public Query Query { get; set; }
        public List<string> Errors { get; set; }

        internal Method Clone()
        {
            return new Method()
            {
                Name = this.Name,
                ParameterTypes = new Dictionary<string, ParameterType>(this.ParameterTypes, StringComparer.OrdinalIgnoreCase),
                Parameters = new Dictionary<string, Parameter>(this.Parameters, StringComparer.OrdinalIgnoreCase),
                ConnectionString = this.ConnectionString,
                Query = this.Query,
                // Don't clone the errors:
                Errors = new List<string>(5)
            };
        }
    }

    class MethodSerialized
    {
        [JsonIgnore]
        readonly Method desc;
        [JsonIgnore]
        readonly bool elideDupeConnectionString;

        internal MethodSerialized(Method desc, bool elideDupeConnectionString = true)
        {
            this.desc = desc;
            this.elideDupeConnectionString = elideDupeConnectionString;
        }

        [JsonProperty("deprecated", NullValueHandling = NullValueHandling.Ignore)]
        public string DeprecatedMessage { get { return desc.DeprecatedMessage; } }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, ParameterSerialized> Parameters
        {
            get
            {
                if (desc.Parameters == null || desc.Parameters.Count == 0)
                    return null;
                return desc.Parameters.ToDictionary(p => p.Key, p => new ParameterSerialized(p.Value), StringComparer.OrdinalIgnoreCase);
            }
        }

        [JsonProperty("connection", NullValueHandling = NullValueHandling.Ignore)]
        public string ConnectionString
        {
            get
            {
                if (elideDupeConnectionString && desc.ConnectionString == desc.Service.ConnectionString) return null;
                return desc.ConnectionString;
            }
        }

        [JsonProperty("sql", NullValueHandling = NullValueHandling.Ignore)]
        public string SQL
        {
            get
            {
                if (desc.Query == null)
                    return null;
                return desc.Query.SQL;
            }
        }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors
        {
            get
            {
                if (desc.Errors == null || desc.Errors.Count == 0)
                    return null;
                return desc.Errors;
            }
        }
    }
}
