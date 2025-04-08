namespace CCIMS.Web.Models.DTOs
{
  public class ResponseDto
  {
    public object? Result { get; set; } = null;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;

  }

  public class ResponseDto<T> : ResponseDto where T : class
  {
    public new T? Result
    {
      get => (T?)base.Result;
      set => base.Result = value;
    }
  }
}
