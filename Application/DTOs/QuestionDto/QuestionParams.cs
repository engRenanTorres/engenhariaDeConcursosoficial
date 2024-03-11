using Apllication.Core;

namespace Apllication.DTO;

public class QuestionParams : PagingParams
{
  public int? MinYear { get; set; }
}
