[Knockout.js](http://knockoutjs.com/documentation/introduction.html) - is a popular JavaScript library that allows easy creation of feature-rich applications based on Model-View-View Model (MVVM) pattern: user interface can be bound to a separate existing data model. And any change of the model will result in dynamic refresh of the interface.

[Knockout MVC](http://knockoutmvc.com) is a library for [ASP.NET MVC3](http://www.asp.net/mvc/mvc3)/[MVC4](http://www.asp.net/mvc/mvc4) , that is a wrapper for Knockout.js, that helps to move entire business logic to the server side: the complete JavaScript code necessary on the client side will be generated automatically based on the described C# (or VB.NET) model. Binding of the page interface to business model is done [MVVM-style](http://knockoutjs.com/documentation/observables.html#mvvm_and_view_models) with the help of C#/VB.NET expressions (not separate properties, but expressions over them that will be translated to JavaScript code) using IntelliSense. If complex operations should be done to the model, it is possible to address to any model method on the server using one short string (ajax query will be automatically generated; and when the client will get the updated model, the whole interface will automatically refresh).

Generated JavaScript code is based on Knockout.js and that is why it works under any browser (even IE 6). Thus, describing the whole business logic on the server in a single instance we get an ability to create **fully-featured cross-browser client Web application without writing a single JavaScript code!**

## Getting Started
* [Project homepage](http://knockoutmvc.com/)
* [NuGet](http://www.nuget.org/packages/kMVC/)
* [Introduction](http://knockoutmvc.com/Home/Introduction)
* [QuickStart](http://knockoutmvc.com/Home/QuickStart)
* [Documentation](http://knockoutmvc.com/Home/Documentation)
* [Forum](https://groups.google.com/forum/#!forum/knockout-mvc)
* [Release notes](ReleaseNotes.md)

## License
MIT license - [http://www.opensource.org/licenses/mit-license.php](http://www.opensource.org/licenses/mit-license.php)

## Copyright
[© 2012–2014 Perpetuum Software LLC](http://www.perpetuumsoft.com/)