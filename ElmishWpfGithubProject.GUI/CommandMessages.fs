module CommandMessages

open DevExpress.Mvvm
open DevExpress.Xpf.Grid

type Msg =
    | ResourceView
    | ProjectView
    | AddPerson
    | CmdNone
    | CloseNewTestWindow of GridControl
    | NewTestWindow2Message of NewTestWindowModule.NewTestWindowMsg
    | NewTestWindowShow

let convertProjectViewParameters (p: obj) =
    try
        let (serviceObject: obj, dataContext: obj) =
            let param = p :?> obj array
            param[0], param[1]
    
        let service = serviceObject :?> INavigationService
    
        service.Navigate(
            viewType = "ElmishWpfGithubProject.ProjectView",
            viewModel = dataContext,
            param = null,
            parentViewModel = null,
            saveToJournal = true
        )
    
        ProjectView
    with
    | e ->
        CmdNone

let convertResourceViewParameters (p: obj) =
    try
        let (serviceObject: obj, dataContext: obj) =
            let param = p :?> obj array
            param[0], param[1]
    
        let service = serviceObject :?> INavigationService
    
        service.Navigate(
            viewType = "ElmishWpfGithubProject.ResourceView",
            viewModel = dataContext,
            param = null,
            parentViewModel = null,
            saveToJournal = true
        )
    
        ProjectView
    with
    | e ->
        CmdNone
