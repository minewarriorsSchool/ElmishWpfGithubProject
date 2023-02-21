namespace ElmishWpfGithubProject.GUI.Converters

open System
open System.Windows.Markup
open System.Windows.Data

type NavigationServiceAndDataContext() =
    inherit MarkupExtension()
    override this.ProvideValue(_serviceProvider: IServiceProvider) = this

    interface IMultiValueConverter with
        member this.Convert(values: obj[], _: Type, _: obj, _: Globalization.CultureInfo): obj = 
            //0 = navigationframeservice, 1= datacontext
            values.Clone()
        member this.ConvertBack(_: obj, _: Type[], _: obj, _: Globalization.CultureInfo): obj[] = 
            raise (System.NotImplementedException())