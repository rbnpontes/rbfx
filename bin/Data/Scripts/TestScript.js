const cache = getSubsystem("ResourceCache");

const font = cache.getResource('Font', 'Fonts/Anonymous Pro.ttf');

const text = new Text();
text.text = "Hello World from Duktape";
text.font = font;
text.fontSize = 30;
text.color = new Color(0.0, 1.0, 0.0);

text.horizontalAlignment = HorizontalAlignment.center;
text.verticalAlignment = VerticalAlignment.center;

const ui = getSubsystem("UI");
ui.root.addChild(text);

// Reduce instantiation calls placing color outside method.
const color = new Color(0.0, 0.0, 0.0);
var time = 0;
subscribeToEvent("Update", function(evtType, evtArgs){
    time += 0.1;
    color.r = (Math.cos(time) + 1) / 2;
    color.g = (Math.sin(time) + 1) / 2;
    
    // native objects must set primitive value again to update values
    text.color = color;
});


Reflection.registerComponent(function TestComponent() {
    this.subscribeToEvent('Update', function(){
        console.log('Hello World from update event.')
    });
}, 'TestComponent');

function instantiateComponent() {
    const obj = new TestComponent();
    obj.enabled = false;
    console.log('Component:', obj.type);
}
instantiateComponent();