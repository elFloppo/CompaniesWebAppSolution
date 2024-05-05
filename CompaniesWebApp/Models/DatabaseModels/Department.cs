using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace CompaniesWebApp.Models.DatabaseModels
{
    [XmlRoot]
    /// <summary>
    /// Департамент
    /// </summary>
    public class Department
    {
        public Department() { }

        [SetsRequiredMembers]
        public Department(string name, Guid companyId) 
        {
            Name = name;
            CompanyId = companyId;
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

        [XmlIgnore]
        [ForeignKey(nameof(Company))]
        public required Guid CompanyId { get; set; }

        [XmlIgnore]
        public Company Company { get; set; }

        [XmlArray]
        [XmlArrayItem("Division")]
        public List<Division> Divisions { get; set; }
    }
}
