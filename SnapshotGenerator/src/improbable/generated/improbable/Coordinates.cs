// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.Coordinates in improbable/standard_library.schema.

namespace Improbable
{

public partial struct Coordinates : global::System.IEquatable<Coordinates>, global::Improbable.Collections.IDeepCopyable<Coordinates>
{
  /// <summary>
  /// Field x = 1.
  /// </summary>
  public double x;
  /// <summary>
  /// Field y = 2.
  /// </summary>
  public double y;
  /// <summary>
  /// Field z = 3.
  /// </summary>
  public double z;

  public Coordinates(
      double x,
      double y,
      double z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public static Coordinates Create()
  {
    var _result = new Coordinates();
    return _result;
  }

  public Coordinates DeepCopy()
  {
    var _result = new Coordinates();
    _result.x = x;
    _result.y = y;
    _result.z = z;
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is Coordinates && Equals((Coordinates) _obj);
  }

  public static bool operator==(Coordinates a, Coordinates b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(Coordinates a, Coordinates b)
  {
    return !a.Equals(b);
  }

  public bool Equals(Coordinates _obj)
  {
    return
        x == _obj.x &&
        y == _obj.y &&
        z == _obj.z;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + x.GetHashCode();
    _result = (_result * 977) + y.GetHashCode();
    _result = (_result * 977) + z.GetHashCode();
    return _result;
  }
}

public static class Coordinates_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                           Coordinates _data, global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    {
      _obj.AddDouble(1, _data.x);
    }
    {
      _obj.AddDouble(2, _data.y);
    }
    {
      _obj.AddDouble(3, _data.z);
    }
  }

  public static unsafe Coordinates Read(global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    Coordinates _data;
    {
      _data.x = _obj.GetDouble(1);
    }
    {
      _data.y = _obj.GetDouble(2);
    }
    {
      _data.z = _obj.GetDouble(3);
    }
    return _data;
  }
}

}
