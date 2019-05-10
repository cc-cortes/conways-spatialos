// Generated by SpatialOS codegen. DO NOT EDIT!
// source: Life in demo.schema.

namespace Demo
{

public static class Life_Extensions
{
  public static Life.Data Get(this global::Improbable.Worker.IComponentData<Life> data)
  {
    return (Life.Data) data;
  }

  public static Life.Update Get(this global::Improbable.Worker.IComponentUpdate<Life> update)
  {
    return (Life.Update) update;
  }
}

public partial class Life : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 111;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the Life component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<Life>, global::Improbable.Collections.IDeepCopyable<Data>
  {
    public global::Demo.LifeData Value;

    public Data(global::Demo.LifeData value)
    {
      Value = value;
    }

    public Data(
        bool curIsAlive,
        ulong curSequenceId,
        bool prevIsAlive,
        ulong prevSequenceId)
    {
      Value = new global::Demo.LifeData(
          curIsAlive,
          curSequenceId,
          prevIsAlive,
          prevSequenceId);
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<Life> ToUpdate()
    {
      var update = new Update();
      update.SetCurIsAlive(Value.curIsAlive);
      update.SetCurSequenceId(Value.curSequenceId);
      update.SetPrevIsAlive(Value.prevIsAlive);
      update.SetPrevSequenceId(Value.prevSequenceId);
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the Life component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<Life>, global::Improbable.Collections.IDeepCopyable<Update>
  {
    /// <summary>
    /// Field cur_is_alive = 1.
    /// </summary>
    public global::Improbable.Collections.Option<bool> curIsAlive;
    public Update SetCurIsAlive(bool _value)
    {
      curIsAlive.Set(_value);
      return this;
    }

    /// <summary>
    /// Field cur_sequence_id = 2.
    /// </summary>
    public global::Improbable.Collections.Option<ulong> curSequenceId;
    public Update SetCurSequenceId(ulong _value)
    {
      curSequenceId.Set(_value);
      return this;
    }

    /// <summary>
    /// Field prev_is_alive = 3.
    /// </summary>
    public global::Improbable.Collections.Option<bool> prevIsAlive;
    public Update SetPrevIsAlive(bool _value)
    {
      prevIsAlive.Set(_value);
      return this;
    }

    /// <summary>
    /// Field prev_sequence_id = 4.
    /// </summary>
    public global::Improbable.Collections.Option<ulong> prevSequenceId;
    public Update SetPrevSequenceId(ulong _value)
    {
      prevSequenceId.Set(_value);
      return this;
    }

    public Update DeepCopy()
    {
      var _result = new Update();
      if (curIsAlive.HasValue)
      {
        bool field;
        field = curIsAlive.Value;
        _result.curIsAlive.Set(field);
      }
      if (curSequenceId.HasValue)
      {
        ulong field;
        field = curSequenceId.Value;
        _result.curSequenceId.Set(field);
      }
      if (prevIsAlive.HasValue)
      {
        bool field;
        field = prevIsAlive.Value;
        _result.prevIsAlive.Set(field);
      }
      if (prevSequenceId.HasValue)
      {
        ulong field;
        field = prevSequenceId.Value;
        _result.prevSequenceId.Set(field);
      }
      return _result;
    }

    public global::Improbable.Worker.IComponentData<Life> ToInitialData()
    {
      return new Data(new global::Demo.LifeData(
          curIsAlive.Value,
          curSequenceId.Value,
          prevIsAlive.Value,
          prevSequenceId.Value));
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<Life> _data)
    {
      var _concrete = _data.Get();
      if (curIsAlive.HasValue)
      {
        _concrete.Value.curIsAlive = curIsAlive.Value;
      }
      if (curSequenceId.HasValue)
      {
        _concrete.Value.curSequenceId = curSequenceId.Value;
      }
      if (prevIsAlive.HasValue)
      {
        _concrete.Value.prevIsAlive = prevIsAlive.Value;
      }
      if (prevSequenceId.HasValue)
      {
        _concrete.Value.prevSequenceId = prevSequenceId.Value;
      }
    }
  }

  public partial class Commands
  {
  }

  // Implementation details below here.
  //----------------------------------------------------------------

  public global::Improbable.Worker.CInterop.ComponentVtable Vtable
  {
    get
    {
      unsafe
      {
        var vtable = new global::Improbable.Worker.CInterop.ComponentVtable
        {
          ComponentId = ComponentId,
          UserData = global::System.UIntPtr.Zero,
          CommandRequestFree = global::Improbable.Worker.Internal.ClientHandles.HandleFree,
          CommandRequestCopy = global::Improbable.Worker.Internal.ClientHandles.HandleCopy,
          CommandRequestDeserialize = CommandRequestDeserialize,
          CommandRequestSerialize = CommandRequestSerialize,
          CommandResponseFree = global::Improbable.Worker.Internal.ClientHandles.HandleFree,
          CommandResponseCopy = global::Improbable.Worker.Internal.ClientHandles.HandleCopy,
          CommandResponseDeserialize = CommandResponseDeserialize,
          CommandResponseSerialize = CommandResponseSerialize,
          ComponentDataFree = global::Improbable.Worker.Internal.ClientHandles.HandleFree,
          ComponentDataCopy = global::Improbable.Worker.Internal.ClientHandles.HandleCopy,
          ComponentDataDeserialize = ComponentDataDeserialize,
          ComponentDataSerialize = ComponentDataSerialize,
          ComponentUpdateFree = global::Improbable.Worker.Internal.ClientHandles.HandleFree,
          ComponentUpdateCopy = global::Improbable.Worker.Internal.ClientHandles.HandleCopy,
          ComponentUpdateDeserialize = ComponentUpdateDeserialize,
          ComponentUpdateSerialize = ComponentUpdateSerialize
        };
        return vtable;
      }
    }
  }

  public void InvokeHandler(global::Improbable.Worker.Dynamic.Handler handler)
  {
    handler.Accept<Life>(this);
  }

  private static unsafe bool
  ComponentUpdateDeserialize(global::System.UInt32 componentId,
                             global::System.UIntPtr userData,
                             global::Improbable.Worker.CInterop.SchemaComponentUpdate source,
                             out global::System.UIntPtr handleOut)
  {
    handleOut = global::System.UIntPtr.Zero;
    try
    {
      var data = new Update();
      var fieldsToClear = new global::System.Collections.Generic.HashSet<uint>();
      var fieldsToClearCount = source.GetClearedFieldCount();
      for (uint i = 0; i < fieldsToClearCount; ++i)
      {
         fieldsToClear.Add(source.IndexClearedField(i));
      }
      var fields = source.GetFields();
      if (fields.GetBoolCount(1) > 0)
      {
        bool field;
        {
          field = fields.GetBool(1);
        }
        data.curIsAlive.Set(field);
      }
      if (fields.GetUint64Count(2) > 0)
      {
        ulong field;
        {
          field = fields.GetUint64(2);
        }
        data.curSequenceId.Set(field);
      }
      if (fields.GetBoolCount(3) > 0)
      {
        bool field;
        {
          field = fields.GetBool(3);
        }
        data.prevIsAlive.Set(field);
      }
      if (fields.GetUint64Count(4) > 0)
      {
        ulong field;
        {
          field = fields.GetUint64(4);
        }
        data.prevSequenceId.Set(field);
      }
      var handle = global::Improbable.Worker.Internal.ClientHandles.HandleAlloc();
      *handle = global::Improbable.Worker.Internal.ClientHandles.Instance.CreateHandle(data);
      handleOut = (global::System.UIntPtr) handle;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return false;
    }
    return true;
  }

  private static unsafe bool
  ComponentDataDeserialize(global::System.UInt32 componentId,
                           global::System.UIntPtr userData,
                           global::Improbable.Worker.CInterop.SchemaComponentData source,
                           out global::System.UIntPtr handleOut)
  {
    handleOut = global::System.UIntPtr.Zero;
    try
    {
      var data = new Data(global::Demo.LifeData_Internal.Read(source.GetFields()));
      var handle = global::Improbable.Worker.Internal.ClientHandles.HandleAlloc();
      *handle = global::Improbable.Worker.Internal.ClientHandles.Instance.CreateHandle(data);
      handleOut = (global::System.UIntPtr) handle;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return false;
    }
    return true;
  }

  private static unsafe bool
  CommandRequestDeserialize(global::System.UInt32 componentId,
                            global::System.UIntPtr userData,
                            global::Improbable.Worker.CInterop.SchemaCommandRequest source,
                            out global::System.UIntPtr handleOut)
  {
    handleOut = global::System.UIntPtr.Zero;
    try
    {
      var data = new global::Improbable.Worker.Internal.GenericCommandObject();
      var handle = global::Improbable.Worker.Internal.ClientHandles.HandleAlloc();
      *handle = global::Improbable.Worker.Internal.ClientHandles.Instance.CreateHandle(data);
      handleOut = (global::System.UIntPtr) handle;
      return false;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return false;
    }
  }

  private static unsafe bool
  CommandResponseDeserialize(global::System.UInt32 componentId,
                             global::System.UIntPtr userData,
                             global::Improbable.Worker.CInterop.SchemaCommandResponse source,
                             out global::System.UIntPtr handleOut)
  {
    handleOut = global::System.UIntPtr.Zero;
    try
    {
      var data = new global::Improbable.Worker.Internal.GenericCommandObject();
      var handle = global::Improbable.Worker.Internal.ClientHandles.HandleAlloc();
      *handle = global::Improbable.Worker.Internal.ClientHandles.Instance.CreateHandle(data);
      handleOut = (global::System.UIntPtr) handle;
      return false;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return false;
    }
  }

  private static unsafe global::Improbable.Worker.CInterop.SchemaComponentUpdate?
  ComponentUpdateSerialize(global::System.UInt32 componentId,
                           global::System.UIntPtr userData,
                           global::System.UIntPtr handle)
  {
    try
    {
      var _pool = global::Improbable.Worker.Internal.ClientHandles.Instance.GetGcHandlePool(handle);
      var data = (Update) global::Improbable.Worker.Internal.ClientHandles.Instance.Dereference(handle);
      var updateObject = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(111);
      var fieldsObject = updateObject.GetFields();
      if (data.curIsAlive.HasValue)
      {
        {
          fieldsObject.AddBool(1, data.curIsAlive.Value);
        }
      }
      if (data.curSequenceId.HasValue)
      {
        {
          fieldsObject.AddUint64(2, data.curSequenceId.Value);
        }
      }
      if (data.prevIsAlive.HasValue)
      {
        {
          fieldsObject.AddBool(3, data.prevIsAlive.Value);
        }
      }
      if (data.prevSequenceId.HasValue)
      {
        {
          fieldsObject.AddUint64(4, data.prevSequenceId.Value);
        }
      }
      return updateObject;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return null;
    }
  }

  private static unsafe global::Improbable.Worker.CInterop.SchemaComponentData?
  ComponentDataSerialize(global::System.UInt32 componentId,
                         global::System.UIntPtr userData,
                         global::System.UIntPtr handle)
  {
    try
    {
      var _pool = global::Improbable.Worker.Internal.ClientHandles.Instance.GetGcHandlePool(handle);
      var data = (Data) global::Improbable.Worker.Internal.ClientHandles.Instance.Dereference(handle);
      var dataObject = new global::Improbable.Worker.CInterop.SchemaComponentData(111);
      global::Demo.LifeData_Internal.Write(_pool, data.Value, dataObject.GetFields());
      return dataObject;
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);
      return null;
    }
  }

  private static unsafe global::Improbable.Worker.CInterop.SchemaCommandRequest?
  CommandRequestSerialize(global::System.UInt32 componentId,
                          global::System.UIntPtr userData,
                          global::System.UIntPtr handle)
  {
    try
    {
      var _pool = global::Improbable.Worker.Internal.ClientHandles.Instance.GetGcHandlePool(handle);
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);

    }
    return null;
  }

  private static unsafe global::Improbable.Worker.CInterop.SchemaCommandResponse?
  CommandResponseSerialize(global::System.UInt32 componentId,
                           global::System.UIntPtr userData,
                           global::System.UIntPtr handle)
  {
    try
    {
      var _pool = global::Improbable.Worker.Internal.ClientHandles.Instance.GetGcHandlePool(handle);
    }
    catch (global::System.Exception e)
    {
      global::Improbable.Worker.ClientError.LogClientException(e);

    }
    return null;
  }

}

}
