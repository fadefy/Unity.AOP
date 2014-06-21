namespace Unity.Func

open System

type Hole = 
    static member Of entry exit target =
          entry target; 
          { new IDisposable with member x.Dispose() = exit(target) }
    static member OfTry entry exit target =
        try
          entry target; 
          { new IDisposable with member x.Dispose() = exit(target) }
        with | exn ->
          { new IDisposable with member x.Dispose() = () }
    static member OfTryFinal entry exit targetFactory = 
      try
         let target = targetFactory();
         try
           entry target;
         with | exn ->
           ()
         { new IDisposable with member x.Dispose() = exit(target) }
      with | exn ->
         { new IDisposable with member x.Dispose() = () }
