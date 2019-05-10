// Generated by SpatialOS codegen. DO NOT EDIT!
// source: System in improbable/restricted/system_components.schema.

namespace Improbable.Restricted
{

public static class System_Extensions
{
  public static System.Data Get(this global::Improbable.Worker.IComponentData<System> data)
  {
    return (System.Data) data;
  }

  public static System.Update Get(this global::Improbable.Worker.IComponentUpdate<System> update)
  {
    return (System.Update) update;
  }
}

public partial class System : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 59;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the System component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<System>, global::Improbable.Collections.IDeepCopyable<Data>
  {
    public global::Improbable.Restricted.SystemData Value;

    public Data(global::Improbable.Restricted.SystemData value)
    {
      Value = value;
    }

    public Data()
    {
      Value = new global::Improbable.Restricted.SystemData();
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<System> ToUpdate()
    {
      var update = new Update();
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the System component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<System>, global::Improbable.Collections.IDeepCopyable<Update>
  {
    public Update DeepCopy()
    {
      var _result = new Update();
      return _result;
    }

    public global::Improbable.Worker.IComponentData<System> ToInitialData()
    {
      return new Data(new global::Improbable.Restricted.SystemData());
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<System> _data)
    {
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
    handler.Accept<System>(this);
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
      var data = new Data(global::Improbable.Restricted.SystemData_Internal.Read(source.GetFields()));
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
      var updateObject = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(59);
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
      var dataObject = new global::Improbable.Worker.CInterop.SchemaComponentData(59);
      global::Improbable.Restricted.SystemData_Internal.Write(_pool, data.Value, dataObject.GetFields());
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
