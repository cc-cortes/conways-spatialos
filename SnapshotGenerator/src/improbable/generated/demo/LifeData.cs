// Generated by SpatialOS codegen. DO NOT EDIT!
// source: demo.LifeData in demo.schema.

namespace Demo
{

public partial struct LifeData : global::System.IEquatable<LifeData>, global::Improbable.Collections.IDeepCopyable<LifeData>
{
  /// <summary>
  /// Field cur_is_alive = 1.
  /// </summary>
  public bool curIsAlive;
  /// <summary>
  /// Field cur_sequence_id = 2.
  /// </summary>
  public ulong curSequenceId;
  /// <summary>
  /// Field prev_is_alive = 3.
  /// </summary>
  public bool prevIsAlive;
  /// <summary>
  /// Field prev_sequence_id = 4.
  /// </summary>
  public ulong prevSequenceId;

  public LifeData(
      bool curIsAlive,
      ulong curSequenceId,
      bool prevIsAlive,
      ulong prevSequenceId)
  {
    this.curIsAlive = curIsAlive;
    this.curSequenceId = curSequenceId;
    this.prevIsAlive = prevIsAlive;
    this.prevSequenceId = prevSequenceId;
  }

  public static LifeData Create()
  {
    var _result = new LifeData();
    return _result;
  }

  public LifeData DeepCopy()
  {
    var _result = new LifeData();
    _result.curIsAlive = curIsAlive;
    _result.curSequenceId = curSequenceId;
    _result.prevIsAlive = prevIsAlive;
    _result.prevSequenceId = prevSequenceId;
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is LifeData && Equals((LifeData) _obj);
  }

  public static bool operator==(LifeData a, LifeData b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(LifeData a, LifeData b)
  {
    return !a.Equals(b);
  }

  public bool Equals(LifeData _obj)
  {
    return
        curIsAlive == _obj.curIsAlive &&
        curSequenceId == _obj.curSequenceId &&
        prevIsAlive == _obj.prevIsAlive &&
        prevSequenceId == _obj.prevSequenceId;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + curIsAlive.GetHashCode();
    _result = (_result * 977) + curSequenceId.GetHashCode();
    _result = (_result * 977) + prevIsAlive.GetHashCode();
    _result = (_result * 977) + prevSequenceId.GetHashCode();
    return _result;
  }
}

public static class LifeData_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                           LifeData _data, global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    {
      _obj.AddBool(1, _data.curIsAlive);
    }
    {
      _obj.AddUint64(2, _data.curSequenceId);
    }
    {
      _obj.AddBool(3, _data.prevIsAlive);
    }
    {
      _obj.AddUint64(4, _data.prevSequenceId);
    }
  }

  public static unsafe LifeData Read(global::Improbable.Worker.CInterop.SchemaObject _obj)
  {
    LifeData _data;
    {
      _data.curIsAlive = _obj.GetBool(1);
    }
    {
      _data.curSequenceId = _obj.GetUint64(2);
    }
    {
      _data.prevIsAlive = _obj.GetBool(3);
    }
    {
      _data.prevSequenceId = _obj.GetUint64(4);
    }
    return _data;
  }
}

}
