using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DataEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Metadata { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DynamicDataEntry : DynamicObject
    {
        private readonly DataEntry _entry;
        private readonly IDictionary<string, string> _entryMetadata;
        private readonly IDictionary<string, Type> _schema;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="schema"></param>
        public DynamicDataEntry(DataEntry entry, IDictionary<string, Type> schema)
        {
            _entry = entry;
            _schema = schema;
            //_entryMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(_entry.Metadata).ToDictionary(x => x.Key, x => x.Value);
            _entryMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(_entry.Metadata);
            //_entryMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(_entry.Metadata).ToDictionary(x => x.Key, x => x.Value);

            //var list = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(_entry.Metadata);
            //_entryMetadata = list.ToDictionary(x => x.Key, x => x.Value);// Dictionary<string, string>>(_entry.Metadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var members = new List<string>(_entryMetadata.Keys);

            members.Add("Id");
            members.Add("Name");
            members.Add("Description");

            return members;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Metadata")
            {
                result = null;
                return false;
            }

            if (_schema.ContainsKey(binder.Name))
            {
                var typeConverter = TypeDescriptor.GetConverter(_schema[binder.Name]);

                result = typeConverter.ConvertFromString(_entryMetadata[binder.Name]);

                return true;
            }

            var entryProperty = typeof(DataEntry).GetProperty(binder.Name);

            result = entryProperty.GetValue(_entry);
            return true;
        }
    }

}
