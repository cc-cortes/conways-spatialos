// Generated by SpatialOS codegen. DO NOT EDIT!
// source: Metadata in improbable/standard_library.schema.

namespace Improbable
{

public static class Metadata_Extensions
{
  public static Metadata.Data Get(this global::Improbable.Worker.IComponentData<Metadata> data)
  {
    return (Metadata.Data) data;
  }

  public static Metadata.Update Get(this global::Improbable.Worker.IComponentUpdate<Metadata> update)
  {
    return (Metadata.Update) update;
  }
}

public partial class Metadata : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 53;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the Metadata component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<Metadata>, global::Improbable.Collections.IDeepCopyable<Data>
  {
    public global::Improbable.MetadataData Value;

    public Data(global::Improbable.MetadataData value)
    {
      Value = value;
    }

    public Data(string entityType)
    {
      Value = new global::Improbable.MetadataData(entityType);
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<Metadata> ToUpdate()
    {
      var update = new Update();
      update.SetEntityType(Value.entityType);
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the Metadata component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<Metadata>, global::Improbable.Collections.IDeepCopyable<Update>
  {
    /// <summary>
    /// Field entity_type = 1.
    /// </summary>
    public global::Improbable.Collections.Option<string> entityType;
    public Update SetEntityType(string _value)
    {
      entityType.Set(_value);
      return this;
    }

    public Update DeepCopy()
    {
      var _result = new Update();
      if (entityType.HasValue)
      {
        string field;
        field = entityType.Value;
        _result.entityType.Set(field);
      }
      return _result;
    }

    public global::Improbable.Worker.IComponentData<Metadata> ToInitialData()
    {
      return new Data(new global::Improbable.MetadataData(entityType.Value));
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<Metadata> _data)
    {
      var _concrete = _data.Get();
      if (entityType.HasValue)
      {
        _concrete.Value.entityType = entityType.Value;
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
    handler.Accept<Metadata>(this);
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
      if (fields.GetStringCount(1) > 0)
      {
        string field;
        {
          field = fields.GetString(1);
        }
        data.entityType.Set(field);
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
      var data = new Data(global::Improbable.MetadataData_Internal.Read(source.GetFields()));
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
      var updateObject = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(53);
      var fieldsObject = updateObject.GetFields();
      if (data.entityType.HasValue)
      {
        {
          fieldsObject.AddString(1, data.entityType.Value);
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
      var dataObject = new global::Improbable.Worker.CInterop.SchemaComponentData(53);
      global::Improbable.MetadataData_Internal.Write(_pool, data.Value, dataObject.GetFields());
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
