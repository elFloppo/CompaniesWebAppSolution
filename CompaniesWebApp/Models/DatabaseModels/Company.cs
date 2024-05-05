using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace CompaniesWebApp.Models.DatabaseModels
{
    [XmlRoot]
    /// <summary>
    /// Компания
    /// </summary>
    public class Company
    {
        public Company() { }

        [SetsRequiredMembers]
        public Company(string name) 
        {
            Name = name;
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
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _name = value;
            }
        }

        [XmlArray]
        [XmlArrayItem("Department")]
        public List<Department> Departments { get; set; }
    }
}
