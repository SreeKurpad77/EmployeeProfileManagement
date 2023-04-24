using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeProfileManagement.Core.Model
{
    public class BlobStorageResult
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobStorageRequest Blob { get; set; }

        public BlobStorageResult()
        {
            Blob = new BlobStorageRequest();
        }
    }
}
