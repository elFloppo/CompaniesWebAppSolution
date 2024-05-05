using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace CompaniesWebApp.Models.DatabaseModels
{
    [XmlRoot]
    /// <summary>
    /// Отдел
    /// </summary>
    public class Division
    {
        public Division() { }

        [SetsRequiredMembers]
        public Division(string name, Guid departmentId)
        {
            Name = name;
            DepartmentId = departmentId;
        }

        [Key]
        [XmlIgnore]
        public Guid? Id { get; set; }

        private string _name;
        [XmlElement]
        public required string Name 
        {
            get => _name; 
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException();

                _name = value;
            }
        }

        [XmlIgnore]
        [ForeignKey(nameof(Department))]
        public required Guid DepartmentId {  get; set; }

        [XmlIgnore]
        public Department Department { get; set; }
    }
}
