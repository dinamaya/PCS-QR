namespace CCIMS.Web.Models.Interfaces
{
    public interface ICreatableByUser : ICreatable
    {
        public string CreatedBy { get; set; }
    }
}
