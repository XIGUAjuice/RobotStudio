//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


class gluon_ikPINVOKE {

  protected class SWIGExceptionHelper {

    public delegate void ExceptionDelegate(string message);
    public delegate void ExceptionArgumentDelegate(string message, string paramName);

    static ExceptionDelegate applicationDelegate = new ExceptionDelegate(SetPendingApplicationException);
    static ExceptionDelegate arithmeticDelegate = new ExceptionDelegate(SetPendingArithmeticException);
    static ExceptionDelegate divideByZeroDelegate = new ExceptionDelegate(SetPendingDivideByZeroException);
    static ExceptionDelegate indexOutOfRangeDelegate = new ExceptionDelegate(SetPendingIndexOutOfRangeException);
    static ExceptionDelegate invalidCastDelegate = new ExceptionDelegate(SetPendingInvalidCastException);
    static ExceptionDelegate invalidOperationDelegate = new ExceptionDelegate(SetPendingInvalidOperationException);
    static ExceptionDelegate ioDelegate = new ExceptionDelegate(SetPendingIOException);
    static ExceptionDelegate nullReferenceDelegate = new ExceptionDelegate(SetPendingNullReferenceException);
    static ExceptionDelegate outOfMemoryDelegate = new ExceptionDelegate(SetPendingOutOfMemoryException);
    static ExceptionDelegate overflowDelegate = new ExceptionDelegate(SetPendingOverflowException);
    static ExceptionDelegate systemDelegate = new ExceptionDelegate(SetPendingSystemException);

    static ExceptionArgumentDelegate argumentDelegate = new ExceptionArgumentDelegate(SetPendingArgumentException);
    static ExceptionArgumentDelegate argumentNullDelegate = new ExceptionArgumentDelegate(SetPendingArgumentNullException);
    static ExceptionArgumentDelegate argumentOutOfRangeDelegate = new ExceptionArgumentDelegate(SetPendingArgumentOutOfRangeException);

    [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="SWIGRegisterExceptionCallbacks_gluon_ik")]
    public static extern void SWIGRegisterExceptionCallbacks_gluon_ik(
                                ExceptionDelegate applicationDelegate,
                                ExceptionDelegate arithmeticDelegate,
                                ExceptionDelegate divideByZeroDelegate, 
                                ExceptionDelegate indexOutOfRangeDelegate, 
                                ExceptionDelegate invalidCastDelegate,
                                ExceptionDelegate invalidOperationDelegate,
                                ExceptionDelegate ioDelegate,
                                ExceptionDelegate nullReferenceDelegate,
                                ExceptionDelegate outOfMemoryDelegate, 
                                ExceptionDelegate overflowDelegate, 
                                ExceptionDelegate systemExceptionDelegate);

    [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_gluon_ik")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_gluon_ik(
                                ExceptionArgumentDelegate argumentDelegate,
                                ExceptionArgumentDelegate argumentNullDelegate,
                                ExceptionArgumentDelegate argumentOutOfRangeDelegate);

    static void SetPendingApplicationException(string message) {
      SWIGPendingException.Set(new global::System.ApplicationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArithmeticException(string message) {
      SWIGPendingException.Set(new global::System.ArithmeticException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingDivideByZeroException(string message) {
      SWIGPendingException.Set(new global::System.DivideByZeroException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIndexOutOfRangeException(string message) {
      SWIGPendingException.Set(new global::System.IndexOutOfRangeException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidCastException(string message) {
      SWIGPendingException.Set(new global::System.InvalidCastException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidOperationException(string message) {
      SWIGPendingException.Set(new global::System.InvalidOperationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIOException(string message) {
      SWIGPendingException.Set(new global::System.IO.IOException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingNullReferenceException(string message) {
      SWIGPendingException.Set(new global::System.NullReferenceException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOutOfMemoryException(string message) {
      SWIGPendingException.Set(new global::System.OutOfMemoryException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOverflowException(string message) {
      SWIGPendingException.Set(new global::System.OverflowException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingSystemException(string message) {
      SWIGPendingException.Set(new global::System.SystemException(message, SWIGPendingException.Retrieve()));
    }

    static void SetPendingArgumentException(string message, string paramName) {
      SWIGPendingException.Set(new global::System.ArgumentException(message, paramName, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArgumentNullException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentNullException(paramName, message));
    }
    static void SetPendingArgumentOutOfRangeException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentOutOfRangeException(paramName, message));
    }

    static SWIGExceptionHelper() {
      SWIGRegisterExceptionCallbacks_gluon_ik(
                                applicationDelegate,
                                arithmeticDelegate,
                                divideByZeroDelegate,
                                indexOutOfRangeDelegate,
                                invalidCastDelegate,
                                invalidOperationDelegate,
                                ioDelegate,
                                nullReferenceDelegate,
                                outOfMemoryDelegate,
                                overflowDelegate,
                                systemDelegate);

      SWIGRegisterExceptionCallbacksArgument_gluon_ik(
                                argumentDelegate,
                                argumentNullDelegate,
                                argumentOutOfRangeDelegate);
    }
  }

  protected static SWIGExceptionHelper swigExceptionHelper = new SWIGExceptionHelper();

  public class SWIGPendingException {
    [global::System.ThreadStatic]
    private static global::System.Exception pendingException = null;
    private static int numExceptionsPending = 0;
    private static global::System.Object exceptionsLock = null;

    public static bool Pending {
      get {
        bool pending = false;
        if (numExceptionsPending > 0)
          if (pendingException != null)
            pending = true;
        return pending;
      } 
    }

    public static void Set(global::System.Exception e) {
      if (pendingException != null)
        throw new global::System.ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + pendingException.ToString() + ")", e);
      pendingException = e;
      lock(exceptionsLock) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(exceptionsLock) {
            numExceptionsPending--;
          }
        }
      }
      return e;
    }

    static SWIGPendingException() {
      exceptionsLock = new global::System.Object();
    }
  }


  protected class SWIGStringHelper {

    public delegate string SWIGStringDelegate(string message);
    static SWIGStringDelegate stringDelegate = new SWIGStringDelegate(CreateString);

    [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="SWIGRegisterStringCallback_gluon_ik")]
    public static extern void SWIGRegisterStringCallback_gluon_ik(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_gluon_ik(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static gluon_ikPINVOKE() {
  }


  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_Gluon_rad")]
  public static extern double Gluon_rad(double jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_Gluon_deg")]
  public static extern double Gluon_deg(double jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_Gluon_poseTotrans")]
  public static extern global::System.IntPtr Gluon_poseTotrans(double jarg1, double jarg2, double jarg3, double jarg4, double jarg5, double jarg6);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_Gluon_IkSolver")]
  public static extern global::System.IntPtr Gluon_IkSolver(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_Gluon_findNearestSolution")]
  public static extern global::System.IntPtr Gluon_findNearestSolution(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_Gluon")]
  public static extern global::System.IntPtr new_Gluon();

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_delete_Gluon")]
  public static extern void delete_Gluon(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Clear")]
  public static extern void DoubleVector_Clear(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Add")]
  public static extern void DoubleVector_Add(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_size")]
  public static extern uint DoubleVector_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_capacity")]
  public static extern uint DoubleVector_capacity(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_reserve")]
  public static extern void DoubleVector_reserve(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector__SWIG_0")]
  public static extern global::System.IntPtr new_DoubleVector__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector__SWIG_1")]
  public static extern global::System.IntPtr new_DoubleVector__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector__SWIG_2")]
  public static extern global::System.IntPtr new_DoubleVector__SWIG_2(int jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_getitemcopy")]
  public static extern double DoubleVector_getitemcopy(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_getitem")]
  public static extern double DoubleVector_getitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_setitem")]
  public static extern void DoubleVector_setitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, double jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_AddRange")]
  public static extern void DoubleVector_AddRange(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_GetRange")]
  public static extern global::System.IntPtr DoubleVector_GetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Insert")]
  public static extern void DoubleVector_Insert(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, double jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_InsertRange")]
  public static extern void DoubleVector_InsertRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_RemoveAt")]
  public static extern void DoubleVector_RemoveAt(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_RemoveRange")]
  public static extern void DoubleVector_RemoveRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Repeat")]
  public static extern global::System.IntPtr DoubleVector_Repeat(double jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Reverse__SWIG_0")]
  public static extern void DoubleVector_Reverse__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Reverse__SWIG_1")]
  public static extern void DoubleVector_Reverse__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_SetRange")]
  public static extern void DoubleVector_SetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Contains")]
  public static extern bool DoubleVector_Contains(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_IndexOf")]
  public static extern int DoubleVector_IndexOf(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_LastIndexOf")]
  public static extern int DoubleVector_LastIndexOf(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector_Remove")]
  public static extern bool DoubleVector_Remove(global::System.Runtime.InteropServices.HandleRef jarg1, double jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_delete_DoubleVector")]
  public static extern void delete_DoubleVector(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Clear")]
  public static extern void DoubleVector2d_Clear(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Add")]
  public static extern void DoubleVector2d_Add(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_size")]
  public static extern uint DoubleVector2d_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_capacity")]
  public static extern uint DoubleVector2d_capacity(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_reserve")]
  public static extern void DoubleVector2d_reserve(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector2d__SWIG_0")]
  public static extern global::System.IntPtr new_DoubleVector2d__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector2d__SWIG_1")]
  public static extern global::System.IntPtr new_DoubleVector2d__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_new_DoubleVector2d__SWIG_2")]
  public static extern global::System.IntPtr new_DoubleVector2d__SWIG_2(int jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_getitemcopy")]
  public static extern global::System.IntPtr DoubleVector2d_getitemcopy(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_getitem")]
  public static extern global::System.IntPtr DoubleVector2d_getitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_setitem")]
  public static extern void DoubleVector2d_setitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_AddRange")]
  public static extern void DoubleVector2d_AddRange(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_GetRange")]
  public static extern global::System.IntPtr DoubleVector2d_GetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Insert")]
  public static extern void DoubleVector2d_Insert(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_InsertRange")]
  public static extern void DoubleVector2d_InsertRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_RemoveAt")]
  public static extern void DoubleVector2d_RemoveAt(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_RemoveRange")]
  public static extern void DoubleVector2d_RemoveRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Repeat")]
  public static extern global::System.IntPtr DoubleVector2d_Repeat(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Reverse__SWIG_0")]
  public static extern void DoubleVector2d_Reverse__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_Reverse__SWIG_1")]
  public static extern void DoubleVector2d_Reverse__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_DoubleVector2d_SetRange")]
  public static extern void DoubleVector2d_SetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("gluon_ik", EntryPoint="CSharp_delete_DoubleVector2d")]
  public static extern void delete_DoubleVector2d(global::System.Runtime.InteropServices.HandleRef jarg1);
}
