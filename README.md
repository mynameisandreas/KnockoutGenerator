KnockoutGenerator
=================

This is the source code for: http://visualstudiogallery.msdn.microsoft.com/32c15a80-1c54-4e96-a83f-7cd57573a5d2 

Knockout Generator:
Adds a new Context menu item to your .cs/.vb files. Right click on a .cs/.vb (inside code window or in the solution window) file that has public properties, klick the new command "Generate Knockout ViewModel".

![](http://blog.andreasgustafsson.se/wp-content/uploads/2013/08/right-click-example.png)

right click example

When you have clicked the new command a new window will popup with options for the new Knockout ViewModel.

![](http://blog.andreasgustafsson.se/wp-content/uploads/2013/08/options-camelcase.png)

options-camelcase

Here is how it works, the grid on the left side shows you all the properties in the Class, the textblock on the right side shows you a preview of what will be generated.
If you checkes a chebox the preview will update immediately.

Observable Checkboxes: Will make the property observable, list, arrays will be observableArray, ordinary properties will be observable
Ignore Checboxes: Will ignore the property and will not be generated

Here in the example I made the property "foo" and "fancypants", observables, because "fancyPants" is a list, it will be a observableArray, "foo" are a simple property and will therefore be an ordinary observable.


When you are done, click the "Copy to clipboard" button, and you will get exactly what's in the preview box in your clipboard, paste it where you want it,
Here is my paste:

function Example(example) 
{
	var self = this;
	self.foo = ko.observable(example.foo || '');
	self.bar = example.bar;
	self.stringArray = example.stringArray;
	self.fancyPants = ko.observableArray(example.fancyPants || [ ]);
}
Knockout generator are easy to use and very convenient to use.



Properties that is private will not be included.
