namespace ElmishWpfGithubProject.GUI

open System.Collections.ObjectModel
open Elmish.WPF
open Elmish
open FSharp.Core
open System.Linq

open CommandMessages
open NewTestWindowModule
open System
open System.Windows

module Model =
    type Person = { Name: string; Age: int }

    type Model =
        { PersonObserverbleCollection: ObservableCollection<Person>
          NewTestWindow: NewTestWindowModule.NewTestWindow option }

    module TestWindow =
        let get m = m.NewTestWindow
        let set v m = { m with NewTestWindow = v }
        let map = map get set

        let mapOutMsg =
            function
            | NewTestWindowModule.Window2OutMsg.Close GridControl -> CloseNewTestWindow GridControl

        let mapInOutMsg = InOut.cata NewTestWindow2Message mapOutMsg

    let init () =
        { PersonObserverbleCollection =
            new ObservableCollection<Person>(
                [ 0..10 ]
                    .Select(fun x -> { Name = $"Person {x}"; Age = x })
            )
          NewTestWindow = None },
        Cmd.none

    let update msg model =
        match msg with
        | AddPerson ->
            model.PersonObserverbleCollection.Add({ Name = "New Person"; Age = 0 })
            model, Cmd.none
        | ResourceView -> model, Cmd.none
        | ProjectView -> model, Cmd.none
        | CmdNone -> model, Cmd.none
        | CloseNewTestWindow gridControl -> 
            (model |> TestWindow.set None)
            model, Cmd.none
        | NewTestWindow2Message msg ->
            model
            |> (msg
                |> TestWindow.update
                |> Option.map
                |> TestWindow.map),
            Cmd.none
        | NewTestWindowShow ->
            model
            |> (new ObservableCollection<string>(model.PersonObserverbleCollection.Select(fun x -> x.Name))
                |> TestWindow.init
                |> Some
                |> TestWindow.set),
            Cmd.none

    let bindings (createTestWindow: unit -> #Window) () =
        [ "BindableCollection"
          |> Binding.oneWay (fun (model: Model) -> model.PersonObserverbleCollection)
          "SwitchProjectView"
          |> Binding.cmdParam convertProjectViewParameters
          "SwitchResourceView"
          |> Binding.cmdParam convertResourceViewParameters
          "NewTestWindow"
          |> Binding.subModelWin (
              TestWindow.get >> WindowState.ofOption,
              snd,
              TestWindow.mapInOutMsg,
              TestWindow.bindings,
              createTestWindow,
              isModal = true
          )
          "ShowNewTestWindow" |> Binding.cmd NewTestWindowShow ]

module Main =

    let main mainWindow (createTestWindow: Func<#Window>) =

        let createTestWindow () =
            let window = createTestWindow.Invoke()
            window.Owner <- mainWindow
            window

        let bindings = Model.bindings createTestWindow

        WpfProgram.mkProgram Model.init Model.update bindings
        |> WpfProgram.startElmishLoop mainWindow
