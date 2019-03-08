// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.WorkerRequirementSet in improbable/standard_library.schema.

namespace Improbable
{

public partial struct WorkerRequirementSet : global::System.IEquatable<WorkerRequirementSet>, global::Improbable.Collections.IDeepCopyable<WorkerRequirementSet>
{
  /// <summary>
  /// Field attribute_set = 1.
  /// </summary>
  public global::Improbable.Collections.List<global::Improbable.WorkerAttributeSet> attributeSet;

  public WorkerRequirementSet(global::Improbable.Collections.List<global::Improbable.WorkerAttributeSet> attributeSet)
  {
    this.attributeSet = attributeSet;
  }

  public static WorkerRequirementSet Create()
  {
    var _result = new WorkerRequirementSet();
    _result.attributeSet = new global::Improbable.Collections.List<global::Improbable.WorkerAttributeSet>();
    return _result;
  }

  public WorkerRequirementSet DeepCopy()
  {
    var _result = new WorkerRequirementSet();
    _result.attributeSet = attributeSet.DeepCopy();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is WorkerRequirementSet && Equals((WorkerRequirementSet) _obj);
  }

  public static bool operator==(WorkerRequirementSet a, WorkerRequirementSet b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(WorkerRequirementSet a, WorkerRequirementSet b)
  {
    return !a.Equals(b);
  }

  public bool Equals(WorkerRequirementSet _obj)
  {
    return
        attributeSet == _obj.attributeSet;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + (attributeSet == null ? 0 : attributeSet.GetHashCode());
    return _result;
  }
}

public static class WorkerRequirementSet_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                           WorkerRequirementSet _data, global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    if (_data.attributeSet != null)
    {
      for (int _i = 0; _i < _data.attributeSet.Count; ++_i)
      {
        global::Improbable.WorkerAttributeSet_Internal.Write(_pool, _data.attributeSet[_i], _obj.AddObject(1));
      }
    }
  }

  public static unsafe WorkerRequirementSet Read(global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    WorkerRequirementSet _data;
    {
      var _count = _obj.GetObjectCount(1);
      _data.attributeSet = new global::Improbable.Collections.List<global::Improbable.WorkerAttributeSet>((int) _count);
      for (uint _i = 0; _i < _count; ++_i)
      {
        _data.attributeSet.Add(global::Improbable.WorkerAttributeSet_Internal.Read(_obj.IndexObject(1, _i)));
      }
    }
    return _data;
  }
}

}
