module NewTestWindowModule

open Elmish.WPF
open System.Collections.ObjectModel
open System.Linq
open DevExpress.Xpf.Grid

[<RequireQualifiedAccess>]
type ConfirmState =
    | Submit
    | Cancel
    | Close

type NewTestWindow =
    { Names: ObservableCollection<string>
      ConfirmState: ConfirmState option }

type NewTestWindowMsg =
    | AddName
    | Submit
    | Cancel
    | Close

[<RequireQualifiedAccessAttribute>]
type Window2OutMsg = | Close of GridControl


module TestWindow =
    module Names =
        let get m = m.Names
        let set x m = { m with Names = x }

        let addName v m=
            //create a randomized name with minium 5 characters and max 10 characters and add it to m.Names
            let random = System.Random()

            let name =
                System.String.Concat(
                    Enumerable
                        .Repeat(v, random.Next(5, 10))
                        .Select(
                            function
                            | (s: string) -> s.[random.Next(s.Length)]
                        )
                ) // had to add the type annotation to s:string because the compiler complained about it

            m.Names.Add(name)
            m


    module ConfirmState =
        let set v m = { m with ConfirmState = v }

    let init (names: ObservableCollection<string>) = { Names = names; ConfirmState = None }

    let update =
        function
        | AddName -> "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" |> Names.addName //had to remove NewTestWindow from here because it was already in the type definition
        | Submit -> ConfirmState.Submit |> Some |> ConfirmState.set
        | Cancel -> ConfirmState.Cancel |> Some |> ConfirmState.set
        | Close  -> ConfirmState.Close  |> Some |> ConfirmState.set


    let private confirmStateToMsg confirmState msg m =
        if m.ConfirmState = Some confirmState
        then InOut.Out Window2OutMsg.Close
        else InOut.In msg

    let bindings() =
        let inBindings =
            [
                "AddName" |> Binding.cmd AddName
                "BindableCollectionNewWindowTest" |> Binding.oneWay (fun m -> m.Names)
            ]
            |> Bindings.mapMsg InOut.In
        let inOutBindings =
            [ "Submit" |> Binding.cmd (confirmStateToMsg ConfirmState.Submit Submit)
              "Cancel" |> Binding.cmd (confirmStateToMsg ConfirmState.Cancel Cancel)
              "CloseNewWindow"  |> Binding.cmd (confirmStateToMsg ConfirmState.Close  Close) 
              ]
        inBindings @ inOutBindings
        
