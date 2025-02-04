// Generated by SpatialOS codegen. DO NOT EDIT!
// source: Neighbors in demo.schema.

namespace Demo
{

public static class Neighbors_Extensions
{
  public static Neighbors.Data Get(this global::Improbable.Worker.IComponentData<Neighbors> data)
  {
    return (Neighbors.Data) data;
  }

  public static Neighbors.Update Get(this global::Improbable.Worker.IComponentUpdate<Neighbors> update)
  {
    return (Neighbors.Update) update;
  }
}

public partial class Neighbors : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 112;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the Neighbors component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<Neighbors>, global::Improbable.Collections.IDeepCopyable<Data>
  {
    public global::Demo.NeighborsData Value;

    public Data(global::Demo.NeighborsData value)
    {
      Value = value;
    }

    public Data(global::Improbable.Collections.List<global::Improbable.EntityId> neighborList)
    {
      Value = new global::Demo.NeighborsData(neighborList);
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<Neighbors> ToUpdate()
    {
      var update = new Update();
      update.SetNeighborList(Value.neighborList);
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the Neighbors component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<Neighbors>, global::Improbable.Collections.IDeepCopyable<Update>
  {
    /// <summary>
    /// Field neighbor_list = 1.
    /// </summary>
    public global::Improbable.Collections.Option<global::Improbable.Collections.List<global::Improbable.EntityId>> neighborList;
    public Update SetNeighborList(global::Improbable.Collections.List<global::Improbable.EntityId> _value)
    {
      if (_value == null)
      {
        throw new System.ArgumentNullException(null, "Attempt to send update with null value.");
      }
      neighborList.Set(_value);
      return this;
    }

    public Update DeepCopy()
    {
      var _result = new Update();
      if (neighborList.HasValue)
      {
        global::Improbable.Collections.List<global::Improbable.EntityId> field;
        field = neighborList.Value.DeepCopy();
        _result.neighborList.Set(field);
      }
      return _result;
    }

    public global::Improbable.Worker.IComponentData<Neighbors> ToInitialData()
    {
      return new Data(new global::Demo.NeighborsData(neighborList.Value));
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<Neighbors> _data)
    {
      var _concrete = _data.Get();
      if (neighborList.HasValue)
      {
        _concrete.Value.neighborList = neighborList.Value;
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
    handler.Accept<Neighbors>(this);
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
      if (fields.GetInt64Count(1) > 0 || fieldsToClear.Contains(1))
      {
        global::Improbable.Collections.List<global::Improbable.EntityId> field;
        {
          var _count = fields.GetInt64Count(1);
          field = new global::Improbable.Collections.List<global::Improbable.EntityId>((int) _count);
          for (uint _i = 0; _i < _count; ++_i)
          {
            field.Add(new global::Improbable.EntityId(fields.IndexInt64(1, _i)));
          }
        }
        data.neighborList.Set(field);
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
      var data = new Data(global::Demo.NeighborsData_Internal.Read(source.GetFields()));
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
      var updateObject = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(112);
      var fieldsObject = updateObject.GetFields();
      if (data.neighborList.HasValue)
      {
        if (data.neighborList.Value.Count == 0)
        {
          updateObject.AddClearedField(1);
        }
        if (data.neighborList.Value != null)
        {
          for (int _i = 0; _i < data.neighborList.Value.Count; ++_i)
          {
            fieldsObject.AddInt64(1, data.neighborList.Value[_i].Id);
          }
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
      var dataObject = new global::Improbable.Worker.CInterop.SchemaComponentData(112);
      global::Demo.NeighborsData_Internal.Write(_pool, data.Value, dataObject.GetFields());
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
