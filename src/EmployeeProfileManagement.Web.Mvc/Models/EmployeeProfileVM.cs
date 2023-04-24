using EmployeeProfileManagement.Core.Model;

namespace EmployeeProfileManagement.Web.Models
{
    public class EmployeeProfileVM : EmployeeProfile
    {
        public EmployeeProfileVM() { }
        public byte[] Data { get; set; }
    }
}
