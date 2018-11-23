// Generated by SpatialOS codegen. DO NOT EDIT!
// source: demo.NeighborsData in demo.schema.

namespace Demo
{

public partial struct NeighborsData : global::System.IEquatable<NeighborsData>, global::Improbable.Collections.IDeepCopyable<NeighborsData>
{
  /// <summary>
  /// Field neighbor_list = 1.
  /// </summary>
  public global::Improbable.Collections.List<global::Improbable.EntityId> neighborList;

  public NeighborsData(global::Improbable.Collections.List<global::Improbable.EntityId> neighborList)
  {
    this.neighborList = neighborList;
  }

  public static NeighborsData Create()
  {
    var _result = new NeighborsData();
    _result.neighborList = new global::Improbable.Collections.List<global::Improbable.EntityId>();
    return _result;
  }

  public NeighborsData DeepCopy()
  {
    var _result = new NeighborsData();
    _result.neighborList = neighborList.DeepCopy();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is NeighborsData && Equals((NeighborsData) _obj);
  }

  public static bool operator==(NeighborsData a, NeighborsData b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(NeighborsData a, NeighborsData b)
  {
    return !a.Equals(b);
  }

  public bool Equals(NeighborsData _obj)
  {
    return
        neighborList == _obj.neighborList;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + (neighborList == null ? 0 : neighborList.GetHashCode());
    return _result;
  }
}

public static class NeighborsData_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                                  NeighborsData _data, global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    for (int _i = 0; _i < _data.neighborList.Count; ++_i)
    {
      global::Improbable.Worker.Internal.Pbio.AddInt64(_obj, 1, _data.neighborList[_i].Id);
    }
  }

  public static unsafe NeighborsData Read(global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    NeighborsData _data;
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetInt64Count(_obj, 1);
      _data.neighborList = new global::Improbable.Collections.List<global::Improbable.EntityId>((int) _count);
      for (uint _i = 0; _i < _count; ++_i)
      {
        _data.neighborList.Add(new global::Improbable.EntityId(global::Improbable.Worker.Internal.Pbio.IndexInt64(_obj, 1, _i)));
      }
    }
    return _data;
  }
}

}
