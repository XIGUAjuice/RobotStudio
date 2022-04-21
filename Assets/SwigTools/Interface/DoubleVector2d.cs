//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class DoubleVector2d : global::System.IDisposable, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<DoubleVector>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal DoubleVector2d(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(DoubleVector2d obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~DoubleVector2d() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          gluon_ikPINVOKE.delete_DoubleVector2d(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public DoubleVector2d(global::System.Collections.IEnumerable c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (DoubleVector element in c) {
      this.Add(element);
    }
  }

  public DoubleVector2d(global::System.Collections.Generic.IEnumerable<DoubleVector> c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (DoubleVector element in c) {
      this.Add(element);
    }
  }

  public bool IsFixedSize {
    get {
      return false;
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public DoubleVector this[int index]  {
    get {
      return getitem(index);
    }
    set {
      setitem(index, value);
    }
  }

  public int Capacity {
    get {
      return (int)capacity();
    }
    set {
      if (value < size())
        throw new global::System.ArgumentOutOfRangeException("Capacity");
      reserve((uint)value);
    }
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsSynchronized {
    get {
      return false;
    }
  }

  public void CopyTo(DoubleVector[] array)
  {
    CopyTo(0, array, 0, this.Count);
  }

  public void CopyTo(DoubleVector[] array, int arrayIndex)
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

  public void CopyTo(int index, DoubleVector[] array, int arrayIndex, int count)
  {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (index < 0)
      throw new global::System.ArgumentOutOfRangeException("index", "Value is less than zero");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (count < 0)
      throw new global::System.ArgumentOutOfRangeException("count", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (index+count > this.Count || arrayIndex+count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");
    for (int i=0; i<count; i++)
      array.SetValue(getitemcopy(index+i), arrayIndex+i);
  }

  public DoubleVector[] ToArray() {
    DoubleVector[] array = new DoubleVector[this.Count];
    this.CopyTo(array);
    return array;
  }

  global::System.Collections.Generic.IEnumerator<DoubleVector> global::System.Collections.Generic.IEnumerable<DoubleVector>.GetEnumerator() {
    return new DoubleVector2dEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new DoubleVector2dEnumerator(this);
  }

  public DoubleVector2dEnumerator GetEnumerator() {
    return new DoubleVector2dEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class DoubleVector2dEnumerator : global::System.Collections.IEnumerator
    , global::System.Collections.Generic.IEnumerator<DoubleVector>
  {
    private DoubleVector2d collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public DoubleVector2dEnumerator(DoubleVector2d collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public DoubleVector Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (DoubleVector)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object global::System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        currentObject = collectionRef[currentIndex];
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new global::System.InvalidOperationException("Collection modified.");
      }
    }

    public void Dispose() {
        currentIndex = -1;
        currentObject = null;
    }
  }

  public void Clear() {
    gluon_ikPINVOKE.DoubleVector2d_Clear(swigCPtr);
  }

  public void Add(DoubleVector x) {
    gluon_ikPINVOKE.DoubleVector2d_Add(swigCPtr, DoubleVector.getCPtr(x));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = gluon_ikPINVOKE.DoubleVector2d_size(swigCPtr);
    return ret;
  }

  private uint capacity() {
    uint ret = gluon_ikPINVOKE.DoubleVector2d_capacity(swigCPtr);
    return ret;
  }

  private void reserve(uint n) {
    gluon_ikPINVOKE.DoubleVector2d_reserve(swigCPtr, n);
  }

  public DoubleVector2d() : this(gluon_ikPINVOKE.new_DoubleVector2d__SWIG_0(), true) {
  }

  public DoubleVector2d(DoubleVector2d other) : this(gluon_ikPINVOKE.new_DoubleVector2d__SWIG_1(DoubleVector2d.getCPtr(other)), true) {
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public DoubleVector2d(int capacity) : this(gluon_ikPINVOKE.new_DoubleVector2d__SWIG_2(capacity), true) {
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  private DoubleVector getitemcopy(int index) {
    DoubleVector ret = new DoubleVector(gluon_ikPINVOKE.DoubleVector2d_getitemcopy(swigCPtr, index), true);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private DoubleVector getitem(int index) {
    DoubleVector ret = new DoubleVector(gluon_ikPINVOKE.DoubleVector2d_getitem(swigCPtr, index), false);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, DoubleVector val) {
    gluon_ikPINVOKE.DoubleVector2d_setitem(swigCPtr, index, DoubleVector.getCPtr(val));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(DoubleVector2d values) {
    gluon_ikPINVOKE.DoubleVector2d_AddRange(swigCPtr, DoubleVector2d.getCPtr(values));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public DoubleVector2d GetRange(int index, int count) {
    global::System.IntPtr cPtr = gluon_ikPINVOKE.DoubleVector2d_GetRange(swigCPtr, index, count);
    DoubleVector2d ret = (cPtr == global::System.IntPtr.Zero) ? null : new DoubleVector2d(cPtr, true);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, DoubleVector x) {
    gluon_ikPINVOKE.DoubleVector2d_Insert(swigCPtr, index, DoubleVector.getCPtr(x));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, DoubleVector2d values) {
    gluon_ikPINVOKE.DoubleVector2d_InsertRange(swigCPtr, index, DoubleVector2d.getCPtr(values));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    gluon_ikPINVOKE.DoubleVector2d_RemoveAt(swigCPtr, index);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    gluon_ikPINVOKE.DoubleVector2d_RemoveRange(swigCPtr, index, count);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public static DoubleVector2d Repeat(DoubleVector value, int count) {
    global::System.IntPtr cPtr = gluon_ikPINVOKE.DoubleVector2d_Repeat(DoubleVector.getCPtr(value), count);
    DoubleVector2d ret = (cPtr == global::System.IntPtr.Zero) ? null : new DoubleVector2d(cPtr, true);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    gluon_ikPINVOKE.DoubleVector2d_Reverse__SWIG_0(swigCPtr);
  }

  public void Reverse(int index, int count) {
    gluon_ikPINVOKE.DoubleVector2d_Reverse__SWIG_1(swigCPtr, index, count);
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, DoubleVector2d values) {
    gluon_ikPINVOKE.DoubleVector2d_SetRange(swigCPtr, index, DoubleVector2d.getCPtr(values));
    if (gluon_ikPINVOKE.SWIGPendingException.Pending) throw gluon_ikPINVOKE.SWIGPendingException.Retrieve();
  }

}
