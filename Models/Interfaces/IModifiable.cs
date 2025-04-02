using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Interfaces
{
    public interface IModifiable
    {
        [DataType(DataType.DateTime)] public DateTime DateModified { get; set; }
    }
}
