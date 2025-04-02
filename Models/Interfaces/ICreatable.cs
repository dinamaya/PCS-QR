using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Interfaces
{
    public interface ICreatable
    {
        [DataType(DataType.DateTime)] public DateTime DateCreated { get; set; }
    }
}
