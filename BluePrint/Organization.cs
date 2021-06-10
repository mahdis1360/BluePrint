using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluePrint
{
    [Table( "Organization")]
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeTitle { get; set; }

        public string UnitName { get; set; }

    }
}