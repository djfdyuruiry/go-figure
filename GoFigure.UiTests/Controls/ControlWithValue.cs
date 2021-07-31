using System.ComponentModel;
using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class ControlWithValue
  {
    protected readonly AutomationElement _element;

    public string Value => _element.Name;

    public ControlWithValue(AutomationElement element) => 
      _element = element;

    public T ValueAs<T>()
    {
      var converter = TypeDescriptor.GetConverter(typeof(T));

      if (!converter.CanConvertTo(typeof(T)) || !converter.CanConvertFrom(typeof(string)))
      {
        return default(T);
      }

      return (T)converter.ConvertFromString(Value);
    }
  }
}
