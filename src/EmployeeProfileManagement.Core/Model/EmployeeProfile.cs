using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeProfileManagement.Core.Model
{
    public class EmployeeProfile : EntityBase
    {  
        /// <summary>
        /// Employee Name - First and Last Name
        /// </summary>
        [Required]
        public string Name { get;set; }

        /// <summary>
        /// Date of birth MM/DD/YYYY format
        /// </summary>
        [Required]
        public DateTime DateofBirth { get; set; }
/// <summary>
/// Designation
/// </summary>
        public string Designation { get; set; }
        /// <summary>
        /// Date of joining
        /// </summary>
        public DateTime HireDate { get; set; }
        /// <summary>
        /// Link to Employee photo stored in Azure BLOB storage
        /// </summary>
        public string PhotoUrl { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
