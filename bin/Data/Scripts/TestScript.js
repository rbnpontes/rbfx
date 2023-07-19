const cache = rbfx.getSubsystem("ResourceCache");
const text = new Text();
text.text = "Hello World from Duktape!";
text.font = cache.getResource("Font", "Fonts/Anonymous Pro.ttf");
text.fontSize = 30;
text.color = new Color(0.0, 1.0, 0.0, 1.0);

text.horizontalAlignment = HorizontalAlignment.center;
text.verticalAlignment = VerticalAlignment.center;

const color = new Color(0.0, 0.0, 0.0, 1.0);
var t = 0;

text.subscribeToEvent('Update', function () {
    console.profile('Update');
    color.fromHSV(Math.sin(t), 1.0, 1.0, 1.0);
    text.color = color;
    t += 0.01;
    console.profileEnd('Update');
});

rbfx.getSubsystem("UI").root.addChild(text);

// const TestComponent = rbfx.component(function () {
//     this.name = "Hello World from Test Component";
//     this.onSetEnabled = function () {
//         console.log('Enabled changed by the someone:', this.enabled);
//     };
// }, 'TestComponent');

// rbfx.registerComponent(TestComponent);


function createPromise(id, timeout) {
    return new Promise(function (resolve) {
        setTimeout(function () {
            resolve(id);
        }, timeout);
    });
}
function failPromise(id, timeout) {
    return new Promise(function (_, reject) {
        setTimeout(function () {
            reject(id);
        }, timeout);
    });
}

const promise = createPromise(10, 1000);
promise.then(function(data) {
    console.log('Data:', data);
});
promise.finally(function() {
    console.log('Finally');
});
