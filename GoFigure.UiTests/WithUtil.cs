using System;

namespace GoFigure.UiTests
{
  public class WithUtil<T>
  {
    protected T _self;

    public T Use(Action<T> withFunc)
    {
      withFunc?.Invoke(_self);

      return _self;
    }
  }
}
