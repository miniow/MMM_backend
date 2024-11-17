using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

namespace Domain.Entities
{
    namespace Domain.Entities
    {
        public class DataPipeline
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            //public ICollection<DataSource> DataSources { get; set; } = new List<DataSource>();
        }
    }
    public class Connection
    {
        public object From { get; set; }
        public object To { get; set; }
    }
    public abstract class DataSource
    {
        public Guid Id { get; set; }
        public Guid DataPipelineId { get; set; }
        public DataPipeline DataPipeline { get; set; }
        public string? Columns { get; set; }
    }

    public class FileSource : DataSource
    {
        public string? LastFileName { get; set; } 
    }

}
