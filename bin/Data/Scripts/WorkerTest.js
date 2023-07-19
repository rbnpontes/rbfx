if(rbfx.thread.isMainThread) {
    console.log('this script has not been loaded inside a worker.');
}
else {
    console.log('this script has been loaded inside a worker. Here is thread index: '+rbfx.thread.index);
}

setTimeout(function() {
    console.log('Timeout is called after 1sec on Worker Thread');
}, 1000);