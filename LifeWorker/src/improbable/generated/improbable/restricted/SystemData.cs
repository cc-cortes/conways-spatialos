// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.restricted.SystemData in improbable/restricted/system_components.schema.

namespace Improbable.Restricted
{

public partial struct SystemData : global::System.IEquatable<SystemData>, global::Improbable.Collections.IDeepCopyable<SystemData>
{
  public static SystemData Create()
  {
    var _result = new SystemData();
    return _result;
  }

  public SystemData DeepCopy()
  {
    var _result = new SystemData();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is SystemData && Equals((SystemData) _obj);
  }

  public static bool operator==(SystemData a, SystemData b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(SystemData a, SystemData b)
  {
    return !a.Equals(b);
  }

  public bool Equals(SystemData _obj)
  {
    return true;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    return _result;
  }
}

public static class SystemData_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                           SystemData _data, global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
  }

  public static unsafe SystemData Read(global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    SystemData _data;
    return _data;
  }
}

}
