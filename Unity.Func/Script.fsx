// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Hole.fs"
open Unity.Func

// Define your library scripting code here

let name = "Hugo"
let hello x = System.Console.WriteLine (x + "")
let o = name |> Hole.Of hello hello
System.Threading.Thread.Sleep(1000)
o.Dispose()

